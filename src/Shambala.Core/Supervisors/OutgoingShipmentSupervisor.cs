using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.Models.DTOModel;
using AutoMapper;
using Shambala.Domain;
using Shambala.Core.Contracts.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System;
namespace Shambala.Core.Supervisors
{
    using Exception;
    using Models;
    using Helphers;
    public class OutgoingShipmentSupervisor : IOutgoingShipmentSupervisor
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        byte _gstRate = 18;
        readonly SchemeProductOptions schemeProductOptions;
        public OutgoingShipmentSupervisor(IMapper mapper, IUnitOfWork unitOfWork, IOptions<SchemeProductOptions> schemeProductOptions)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.schemeProductOptions = schemeProductOptions.Value;
        }

        public byte GSTRate => _gstRate;

        public IEnumerable<ProductOutOfStockBLL> CheckPostShipment(IEnumerable<ProductQuantityBLL> productQuantitties, int? Id = null)
        {
            if (productQuantitties.Distinct().Count() != productQuantitties.Count())
                throw new DuplicateShipmentsException();

            if (_unitOfWork.CurrentTransaction == null)
            {
                using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking();
                    OutgoingShipment outgoing = Id == null ? null : _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id.Value);
                    return GetOutOfStockList(outgoing, productQuantitties, products);
                }
            }
            IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAllWithNoTracking();
            OutgoingShipment outgoingShipment = Id == null ? null : _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id.Value);
            return GetOutOfStockList(outgoingShipment, productQuantitties, productList);
        }
        void UpdateQuantities(IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails)
        {
            foreach (var item in outgoingShipmentDetails)
            {
                int QuantityShiped = item.TotalQuantityShiped - item.TotalQuantityRejected;
                int ProductId = item.ProductIdFk; int FlavourId = item.FlavourIdFk;
                if (item.TotalQuantityRejected > 0)
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, item.TotalQuantityRejected);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, QuantityShiped);
            }
        }
        public ResultModel AddAsync(OutgoingShipmentPostDTO postOutgoingShipmentDTO)
        {
            if (!this.IsShipmentsUnique(postOutgoingShipmentDTO.Shipments))
                return new ResultModel() { IsValid = false, Code = ((int)OutgoingErroCode.DUPLICATE), Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.DUPLICATE) };

            OutgoingShipment outgoingShipment = _mapper.Map<OutgoingShipment>(postOutgoingShipmentDTO);
            ResultModel resultModel = new ResultModel();

            using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.CheckPostShipment(postOutgoingShipmentDTO.Shipments.Select(e => new ProductQuantityBLL { FlavourId = e.FlavourId, ProductId = e.ProductId, Quantity = e.TotalRecievedPieces + e.TotalDefectPieces }));

                if (productOutOfStockBLLs == null)
                {
                    this.UpdateQuantities(outgoingShipment.OutgoingShipmentDetails);
                    outgoingShipment = _unitOfWork.OutgoingShipmentRepository.Add(outgoingShipment);
                    _unitOfWork.SaveChanges();
                }
                else
                {
                    _unitOfWork.Rollback();
                    resultModel.IsValid = false;
                    resultModel.Code = ((int)OutgoingErroCode.DUPLICATE);
                    resultModel.Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.DUPLICATE);
                    resultModel.Content = productOutOfStockBLLs;
                    return resultModel;
                }
            }

            _unitOfWork.OutgoingShipmentRepository.Load(outgoingShipment, entity => entity.SalesmanIdFkNavigation);
            resultModel.IsValid = true;
            resultModel.Content = _mapper.Map<OutgoingShipmentInfoDTO>(outgoingShipment);
            return resultModel;
        }
        // bool ClearCredit(ShipmentLedgerDetail shipmentLedgerDetail)
        // {
        //     IEnumerable<LedgerWithPastDebitDTO> ledgers = shipmentLedgerDetail.Ledgers;
        //     IEnumerable<ShopCreditOrDebitDTO> shopRecievedCredits = _mapper.Map<IEnumerable<ShopCreditOrDebitDTO>>(ledgers);
        //     IEnumerable<InvoiceAggreagateDetailBLL> notClearedInvoice = readInvoiceRepository.GetNotClearedAggregateByShopIds(shopRecievedCredits.Select(e => e.ShopId).ToArray());
        //     IEnumerable<ShopCreditOrDebitDTO> creditLeftOverDTO = Utility.CheckDebitUnderGivenBalance(shopRecievedCredits, notClearedInvoice);
        //     bool IsAllOk = creditLeftOverDTO.Count() == 0;
        //     if (IsAllOk)
        //     {
        //         foreach (var ledger in ledgers)
        //         {
        //             decimal totalRecievedDue = ledger.OldDebit;
        //             foreach (var invoice in notClearedInvoice.Where(e => e.ShopId == ledger.ShopId).OrderBy(e => e.Id))
        //             {
        //                 if (totalRecievedDue > 0)
        //                 {
        //                     decimal totalDueLeft = invoice.TotalPrice - invoice.TotalDueCleared;
        //                     decimal amountToCleared = totalDueLeft >= totalRecievedDue ? totalRecievedDue : totalDueLeft;
        //                     _unitOfWork.DebitRepository.Add(shipmentLedgerDetail.Id, ledger.ShopId, amountToCleared, shipmentLedgerDetail.DateCreated);
        //                     totalRecievedDue -= amountToCleared;
        //                     if (Shambala.Helpher.InvoiceTolerance.IsCleared(invoice.TotalPrice, invoice.TotalDueCleared + amountToCleared))
        //                     {
        //                         _unitOfWork.InvoiceRepository.MakeCompleted(invoice.Id);
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     return IsAllOk;
        // }


        bool IsSchemeQuantityAvailable(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetailDTOs, IEnumerable<OutgoingShipmentDetails> oldOutoingDetails, IEnumerable<Product> products)
        {
            bool IsValid = true;
            int schemeQuantityLeft = products.First(e => e.Id == this.schemeProductOptions.ProductId).ProductFlavourQuantity.First(e => e.FlavourIdFk == this.schemeProductOptions.FlavourId).Quantity;
            foreach (OutgoingShipmentDetailTransferDTO currentShipmentDetailDTO in outgoingShipmentDetailDTOs)
            {
                OutgoingShipmentDetails oldDetail = oldOutoingDetails.FirstOrDefault(e => e.ProductIdFk == currentShipmentDetailDTO.ProductId && e.FlavourIdFk == currentShipmentDetailDTO.FlavourId);
                short oldSchemeQuantity = oldDetail?.SchemeTotalQuantity ?? 0;
                short newSchemeQuantity = currentShipmentDetailDTO.SchemeInfo.TotalQuantity;
                schemeQuantityLeft += oldSchemeQuantity;
                schemeQuantityLeft -= newSchemeQuantity;
                if (schemeQuantityLeft < 0)
                {
                    IsValid = false;
                    break;
                }
            }

            return IsValid;
        }
        IEnumerable<ProductFlavourElement> CheckSchemeQuantityValid(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetailDTOs, IEnumerable<Product> products)
        {
            ICollection<ProductFlavourElement> productFlavourElements = new List<ProductFlavourElement>();
            foreach (OutgoingShipmentDetailTransferDTO outgoingDetail in outgoingShipmentDetailDTOs)
            {
                Product product = products.First(e => e.Id == outgoingDetail.ProductId);
                if (Utility.GetTotalSchemeQuantity(outgoingDetail.TotalQuantityShiped, product.CaretSize, outgoingDetail.SchemeInfo.SchemeQuantity) != outgoingDetail.SchemeInfo.TotalQuantity)
                    productFlavourElements.Add(new ProductFlavourElement { FlavourId = outgoingDetail.FlavourId, ProductId = outgoingDetail.ProductId });
            }
            return productFlavourElements.Count > 0 ? productFlavourElements : null;
        }
        IEnumerable<ProductFlavourElement> CheckQuantityShipedValid(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetailDTOs)
        {
            ICollection<ProductFlavourElement> productFlavourElements = new List<ProductFlavourElement>();
            foreach (OutgoingShipmentDetailBaseDTO productFlavourElement in outgoingShipmentDetailDTOs)
                if (productFlavourElement.TotalQuantityTaken - productFlavourElement.TotalQuantityReturned != productFlavourElement.TotalQuantityShiped)
                    productFlavourElements.Add(new ProductFlavourElement { ProductId = productFlavourElement.ProductId, FlavourId = productFlavourElement.FlavourId });
            return productFlavourElements.Count > 0 ? productFlavourElements : null;
        }
        IEnumerable<ProductFlavourElement> CheckCustomCaratPriceValid(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetailDTOs)
        {
            ICollection<ProductFlavourElement> elemtnWithInvalidCaratPriceCollection = new List<ProductFlavourElement>();
            foreach (OutgoingShipmentDetailTransferDTO outgoingShipmentDetail in outgoingShipmentDetailDTOs)
            {
                if (outgoingShipmentDetail.CustomCaratPrices.TotalQuantity != ((short)outgoingShipmentDetail.CustomCaratPrices.Prices.Sum(e => e.Quantity)) || outgoingShipmentDetail.CustomCaratPrices.TotalQuantity > outgoingShipmentDetail.TotalQuantityShiped)
                    elemtnWithInvalidCaratPriceCollection.Add(new ProductFlavourElement { FlavourId = outgoingShipmentDetail.FlavourId, ProductId = outgoingShipmentDetail.ProductId });
            }
            return elemtnWithInvalidCaratPriceCollection.Count > 0 ? elemtnWithInvalidCaratPriceCollection : null;
        }
        IEnumerable<ProductFlavourElement> CheckSchemePriceValid(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetailDTOs, IEnumerable<Product> products)
        {
            ICollection<ProductFlavourElement> result = new List<ProductFlavourElement>();
            Product schemeProduct = products.First(e => e.Id == schemeProductOptions.ProductId);
            decimal pricePerBottle = schemeProduct.PricePerCaret / schemeProduct.CaretSize;
            foreach (OutgoingShipmentDetailTransferDTO detailDTO in outgoingShipmentDetailDTOs)
            {
                if (detailDTO.SchemeInfo.TotalSchemePrice != Utility.GetTotalProductPrice(schemeProduct, detailDTO.SchemeInfo.TotalQuantity))
                {
                    result.Add(new ProductFlavourElement { FlavourId = detailDTO.FlavourId, ProductId = detailDTO.ProductId });
                }
            }
            return result.Count > 0 ? result : null;
        }
        public ResultModel Update(OutgoingShipmentDTO outgoingShipmentDTO)
        {
            IEnumerable<ProductFlavourElement> productFlavourElements = CheckQuantityShipedValid(outgoingShipmentDTO.OutgoingShipmentDetails);
            if (productFlavourElements != null)
                return new ResultModel { Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SHIPED_QUANTITY_NOT_VALID), Code = ((int)OutgoingErroCode.SHIPED_QUANTITY_NOT_VALID), Content = productFlavourElements };
            IEnumerable<ProductFlavourElement> elementWithInvalidCustomPriceCollection = CheckCustomCaratPriceValid(outgoingShipmentDTO.OutgoingShipmentDetails);
            if (elementWithInvalidCustomPriceCollection != null)
            {
                return new ResultModel
                {
                    Code = ((int)OutgoingErroCode.CUSTOM_CARAT_PRICE_NOT_VALID),
                    Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.CUSTOM_CARAT_PRICE_NOT_VALID),
                    Content = elementWithInvalidCustomPriceCollection,
                    IsValid = false
                };
            }
            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(outgoingShipmentDTO.Id);
            bool IsUpdated = _unitOfWork.OutgoingShipmentRepository.Update(outgoingShipment);
            if (!IsUpdated)
            {
                return new ResultModel
                {
                    Code = ((int)ConcurrencyErrorCode.Concurrency_Error),
                    Content = "Another User Already Changed The Content",
                    IsValid = false,
                    Name = "Concurrency Exception"
                };
            }
            outgoingShipment.Status = _mapper.Map<string>(OutgoingShipmentStatus.FILLED);
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking();

            //check Given TotalSchemeQuantity == (QSinCaret)*SchemeQuantityPerCaret
            {
                IEnumerable<ProductFlavourElement> productWithSchemeNotValid = this.CheckSchemeQuantityValid(outgoingShipmentDTO.OutgoingShipmentDetails, products);
                if (productWithSchemeNotValid != null)
                    return new ResultModel { Code = ((int)OutgoingErroCode.SCHEME_QUANTITY_NOT_VALID), Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SCHEME_QUANTITY_NOT_VALID), IsValid = false, Content = productWithSchemeNotValid };
                //check scheme total price valid 
                IEnumerable<ProductFlavourElement> productWithSchemePriceNotValid = this.CheckSchemePriceValid(outgoingShipmentDTO.OutgoingShipmentDetails, products);
                if (productWithSchemePriceNotValid != null)
                    return new ResultModel { Code = (int)OutgoingErroCode.SCHME_PRICE_NOT_VALID, Content = productWithSchemePriceNotValid, Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SCHME_PRICE_NOT_VALID) };

                //check scheme quantity available
                if (!this.IsSchemeQuantityAvailable(outgoingShipmentDTO.OutgoingShipmentDetails, outgoingShipment.OutgoingShipmentDetails, products))
                    return new ResultModel { Code = ((int)OutgoingErroCode.SCHEME_EXCEED), Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SCHEME_EXCEED), Content = "Scheme Product Quantity Exceed" };
            }

            //check quantity available            
            IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.GetOutOfStockList(outgoingShipment, outgoingShipmentDTO.OutgoingShipmentDetails.Select(e => new ProductQuantityBLL { ProductId = e.ProductId, Quantity = e.TotalQuantityShiped - e.TotalQuantityReturned, FlavourId = e.FlavourId }), products);
            if (productOutOfStockBLLs != null)
            {
                return new ResultModel { Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.OUT_OF_STOCK), Code = ((int)OutgoingErroCode.OUT_OF_STOCK), Content = productOutOfStockBLLs };
            }

            Product SchemeProduct = products.First(e => e.Id == this.schemeProductOptions.ProductId);
            int SchemeProductId = this.schemeProductOptions.ProductId;
            short SchemeFlavourId = this.schemeProductOptions.FlavourId;
            //delete shipments
            IEnumerable<OutgoingShipmentDetails> deleteShipments = outgoingShipment.OutgoingShipmentDetails
            .Where(e => !outgoingShipmentDTO.OutgoingShipmentDetails.Any(f => f.FlavourId == e.FlavourIdFk && f.ProductId == e.ProductIdFk && f.OutgoingShipmentId == e.OutgoingShipmentIdFk));
            foreach (var deleteShipment in _mapper.Map<IEnumerable<OutgoingShipmentDetails>>(deleteShipments))
            {
                _unitOfWork.OutgoingShipmentDetailRepository.Delete(deleteShipment.Id);
                _unitOfWork.ProductRepository.AddQuantity(deleteShipment.ProductIdFk, deleteShipment.FlavourIdFk, deleteShipment.TotalQuantityShiped);
                _unitOfWork.ProductRepository.AddQuantity(SchemeProductId, SchemeFlavourId, deleteShipment.SchemeTotalQuantity);
                foreach (var customprice in deleteShipment.CustomCaratPrices)
                    _unitOfWork.CustomPriceRepository.Delete(customprice);
            }
            //new Shipments
            IEnumerable<OutgoingShipmentDetailTransferDTO> newShipments = outgoingShipmentDTO.OutgoingShipmentDetails.Where(e => !outgoingShipment.OutgoingShipmentDetails.Any(f => f.ProductIdFk == e.ProductId && f.FlavourIdFk == e.FlavourId)).ToList();

            foreach (var newShipment in newShipments)
            {
                OutgoingShipmentDetails newShipmentDetails = _mapper.Map<OutgoingShipmentDetails>(newShipment);
                int ProductId = newShipment.ProductId; short FlavourId = newShipment.FlavourId;
                newShipmentDetails.OutgoingShipmentIdFk = outgoingShipment.Id;
                Product product = products.First(e => e.Id == ProductId);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, newShipment.TotalQuantityShiped + newShipment.TotalQuantityRejected);
                short TotalSchemeQuantity = newShipment.SchemeInfo.TotalQuantity;
                //add scheme price and quantity
                newShipmentDetails.SchemeTotalQuantity = ((byte)TotalSchemeQuantity);
                newShipmentDetails.SchemeTotalPrice = Utility.GetTotalProductPrice(SchemeProduct, TotalSchemeQuantity);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductId, SchemeFlavourId, TotalSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Add(newShipmentDetails);
            }

            //update Shipments
            IEnumerable<OutgoingShipmentDetailTransferDTO> updateShipments = outgoingShipmentDTO.OutgoingShipmentDetails
            .Where(e => outgoingShipment.OutgoingShipmentDetails.Any(f => f.FlavourIdFk == e.FlavourId && f.ProductIdFk == e.ProductId));
            foreach (OutgoingShipmentDetailTransferDTO shipment in updateShipments)
            {
                OutgoingShipmentDetails updateShipment = _mapper.Map<OutgoingShipmentDetails>(shipment);
                updateShipment.OutgoingShipmentIdFk = outgoingShipment.Id;
                int ProductId = updateShipment.ProductIdFk; short FlavourId = updateShipment.FlavourIdFk;
                Product product = products.First(e => e.Id == ProductId);
                OutgoingShipmentDetails previousShipment = outgoingShipment.OutgoingShipmentDetails.First(e => e.Id == updateShipment.Id);

                SetNewCustomCaratPrice(previousShipment.CustomCaratPrices, updateShipment.CustomCaratPrices);
                if (previousShipment.TotalQuantityTaken != updateShipment.TotalQuantityTaken)
                {
                    short abolsuteQuantity = (short)Math.Abs(previousShipment.TotalQuantityTaken - updateShipment.TotalQuantityTaken);
                    if (previousShipment.TotalQuantityTaken > updateShipment.TotalQuantityTaken)
                        _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, abolsuteQuantity);
                    else
                        _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, abolsuteQuantity);
                }
                if (previousShipment.TotalQuantityReturned != previousShipment.TotalQuantityReturned)
                {
                    short absoluteQuantity = (short)Math.Abs(previousShipment.TotalQuantityReturned - updateShipment.TotalQuantityReturned);
                    if (previousShipment.TotalQuantityReturned > updateShipment.TotalQuantityReturned)
                        _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, absoluteQuantity);
                    else
                        _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, absoluteQuantity);
                }
                short newSchemeQuantity = Utility.GetTotalSchemeQuantity(updateShipment.TotalQuantityShiped, product.CaretSize, product.SchemeQuantity ?? 0);
                if (previousShipment.SchemeTotalQuantity != newSchemeQuantity)
                {
                    short absoluteQuantity = (short)Math.Abs(previousShipment.SchemeTotalQuantity - newSchemeQuantity);
                    if (previousShipment.SchemeTotalQuantity > newSchemeQuantity)
                        _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, absoluteQuantity);
                    else
                        _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, absoluteQuantity);
                }
                short previousQuantity = previousShipment.TotalQuantityShiped;
                short newQuantity = updateShipment.TotalQuantityShiped;

                updateShipment.SchemeTotalPrice = Utility.GetTotalProductPrice(SchemeProduct, newSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Update(updateShipment);
            }
            _unitOfWork.SaveChanges();
            return new ResultModel { IsValid = true };
        }
        void SetNewCustomCaratPrice(IEnumerable<CustomCaratPrice> oldCustomPries, IEnumerable<CustomCaratPrice> newCustomPrices)
        {
            foreach (CustomCaratPrice customCaratPrice in oldCustomPries)
                _unitOfWork.CustomPriceRepository.Delete(customCaratPrice);
            foreach (CustomCaratPrice customCarat in newCustomPrices)
                _unitOfWork.CustomPriceRepository.Add(customCarat);
        }
        bool IsShipmentsUnique(IEnumerable<ShipmentDTO> shipments)
        {
            return shipments.Distinct(new ShipmentComparer()).Count() == shipments.Count();
        }
        IEnumerable<ProductOutOfStockBLL> GetOutOfStockList(OutgoingShipment outgoingShipment, IEnumerable<ProductQuantityBLL> recieveShipments, IEnumerable<Product> products)
        {
            ICollection<ProductOutOfStockBLL> productOutOfStocks = new List<ProductOutOfStockBLL>();

            foreach (ProductQuantityBLL shipment in recieveShipments)
            {
                int ProductId = shipment.ProductId;
                short FlavourId = shipment.FlavourId;
                int ProductQuantityInStock = products.First(e => e.Id == ProductId).ProductFlavourQuantity.Where(e => e.FlavourIdFk == FlavourId).First().Quantity;
                if (outgoingShipment != null)
                {
                    ProductQuantityInStock += outgoingShipment.OutgoingShipmentDetails.FirstOrDefault(e => e.FlavourIdFk == FlavourId && e.ProductIdFk == ProductId)?.TotalQuantityShiped ?? 0;
                }
                if (shipment.Quantity > ProductQuantityInStock)
                {
                    productOutOfStocks.Add(new ProductOutOfStockBLL() { FlavourId = FlavourId, ProductId = ProductId, Quantity = ProductQuantityInStock });
                }
            }
            return productOutOfStocks.Count() > 0 ? productOutOfStocks : null;
        }

        public OutgoingShipmentInfoDTO GetById(int Id)
        {
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
            OutgoingShipmentInfoDTO outgoingShipmentInfoDTO = _mapper.Map<OutgoingShipmentInfoDTO>(outgoingShipment);
            AfterMapper.OutgoingShipmentTransferDTODetails(outgoingShipmentInfoDTO.OutgoingShipmentDetails, _unitOfWork.ProductRepository.GetAllWithNoTracking());
            return outgoingShipmentInfoDTO;
        }
    }
}