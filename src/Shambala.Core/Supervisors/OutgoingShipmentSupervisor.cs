using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.Models.DTOModel;
using AutoMapper;
using Shambala.Domain;
using Shambala.Core.Contracts.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Shambala.Core.Supervisors
{
    using Exception;
    using Models;
    using Helphers;
    using System;
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

            if (productQuantitties.Distinct().Count() == productQuantitties.Count())
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
                IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.CheckPostShipment(postOutgoingShipmentDTO.Shipments.Select(e => new ProductQuantityBLL { FlavourId = e.FlavourId, ProductId = e.ProductId, Quantity = e.TotalRecievedPieces+e.TotalDefectPieces }));

                if (productOutOfStockBLLs == null)
                {
                    this.UpdateQuantities(outgoingShipment.OutgoingShipmentDetails);
                    _unitOfWork.OutgoingShipmentRepository.Add(outgoingShipment);
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

        IEnumerable<ProductQuantityBLL> GetReturnProductFromShipments(IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails)
        {
            ICollection<ProductQuantityBLL> productReturnBLLs = new List<ProductQuantityBLL>();

            foreach (var OutgoingShipmentDetail in outgoingShipmentDetails)
            {
                if (OutgoingShipmentDetail.TotalQuantityReturned >= 0)
                    productReturnBLLs.Add(new ProductQuantityBLL
                    {
                        ProductId = OutgoingShipmentDetail.ProductIdFk,
                        FlavourId = OutgoingShipmentDetail.FlavourIdFk,
                        Quantity = OutgoingShipmentDetail.TotalQuantityReturned
                    });
            }
            return productReturnBLLs;
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

        // public async Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail)
        // {
        //     int OutgoingShipmentId = shipmentLedgerDetail.Id;
        //     IEnumerable<ShopCreditOrDebitDTO> shopRecievedCredits = _mapper.Map<IEnumerable<ShopCreditOrDebitDTO>>(shipmentLedgerDetail.Ledgers);

        //     OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(OutgoingShipmentId);


        //     IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails = outgoing.OutgoingShipmentDetails;

        //     _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
        //     // if (!this.ClearCredit(shipmentLedgerDetail))
        //     // {
        //     //     return false;
        //     // }
        //     _unitOfWork.OutgoingShipmentRepository.Complete(OutgoingShipmentId);
        //     await _unitOfWork.SaveChangesAsync();
        //     return true;

        // }

        // public OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int orderId)
        // {
        //     IEnumerable<OutgoingShipmentProductInfoDTO> OutgoingShipmentDettailInfos = readOutgoingShipmentRepository.GetProductsById(orderId: orderId);
        //     OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(orderId);

        //     OutgoingShipmentWithProductListDTO outgoingShipmentWithProductListDTO = new OutgoingShipmentWithProductListDTO()
        //     {
        //         Id = outgoing.Id,
        //         DateCreated = outgoing.DateCreated,
        //         Status = (OutgoingShipmentStatus)System.Enum.Parse(typeof(OutgoingShipmentStatus), outgoing.Status),
        //         Salesman = _mapper.Map<SalesmanDTO>(outgoing.SalesmanIdFkNavigation)
        //     };
        //     if (OutgoingShipmentDettailInfos.Count() == 0)
        //     {
        //         outgoingShipmentWithProductListDTO.Products = new List<ProductDTO>();
        //         return outgoingShipmentWithProductListDTO;
        //     }
        //     IEnumerable<ProductDTO> Products = OutgoingShipmentDettailInfos.GroupBy(e => e.Product.Id).First()
        //     .GroupJoin(OutgoingShipmentDettailInfos, e => e.Product.Id, f => f.Product.Id, (e, f) => new ProductDTO()
        //     {
        //         CaretSize = e.Product.CaretSize,
        //         Id = e.Product.Id,
        //         Name = e.Product.Name,
        //         Flavours = f.Select(s => s.Flavour).ToList()
        //     });
        //     outgoingShipmentWithProductListDTO.Products = Products;
        //     return outgoingShipmentWithProductListDTO;
        // }
        // public IEnumerable<ProductOutOfStockBLL> CheckReturnShipment(int Id, IEnumerable<ShipmentDTO> shipments)
        // {
        //     if (_unitOfWork.CurrentTransaction == null)
        //     {
        //         using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //         {
        //             return this.GetOverReturnShipment(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id), shipments);
        //         }
        //     }
        //     return this.GetOverReturnShipment(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id), shipments);
        // }
        IEnumerable<ProductOutOfStockBLL> GetOverReturnShipment(OutgoingShipment outgoingShipment, IEnumerable<ShipmentDTO> shipments)
        {
            ICollection<ProductOutOfStockBLL> productOutOfs = new List<ProductOutOfStockBLL>();
            foreach (ShipmentDTO shipmentDTO in shipments)
            {
                int ProductId = shipmentDTO.ProductId;
                short FlavourId = shipmentDTO.FlavourId;
                OutgoingShipmentDetails outgoingShipmentDetail = outgoingShipment.OutgoingShipmentDetails.FirstOrDefault(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId);
                if (outgoingShipmentDetail == null)
                    throw new ShipmentNotVaidException();
                short ShipedQuantity = outgoingShipmentDetail.TotalQuantityReturned;
                if (shipmentDTO.TotalRecievedPieces > ShipedQuantity)
                    productOutOfs.Add(new ProductOutOfStockBLL() { FlavourId = FlavourId, ProductId = ProductId, Quantity = ShipedQuantity });
            }
            return productOutOfs.Count() > 0 ? productOutOfs : null;
        }

        // public async Task<bool> ReturnShipmentAsync(int Id, IEnumerable<ShipmentDTO> recieveShipments)
        // {

        //     if (IsShipmentsUnique(recieveShipments))
        //         throw new DuplicateShipmentsException();

        //     _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
        //     OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
        //     IEnumerable<ProductOutOfStockBLL> productReturnOverShipeds = this.GetOverReturnShipment(outgoingShipment, recieveShipments);
        //     IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking(outgoingShipment.DateCreated);
        //     Product SchemeProduct = products.First(e => e.Id == this.schemeProductOptions.ProductId);

        //     if (productReturnOverShipeds != null)
        //         throw new ShipmentReturnQuantityExceedException();
        //     if (!this.IsSchemeQuantityAvailable(_mapper.Map<IEnumerable<ShipmentDTO>>(
        //         outgoingShipment.OutgoingShipmentDetails),
        //         recieveShipments, SchemeProduct.ProductFlavourQuantity.First(e => e.FlavourIdFk == this.schemeProductOptions.FlavourId).Quantity + outgoingShipment.OutgoingShipmentDetails.Sum(e => e.SchemeTotalQuantity), products))
        //     {
        //         throw new SchemeProductQuantityExceedException();
        //     }
        //     foreach (ShipmentDTO updateShipment in recieveShipments.Where(e => e.Id != 0))
        //     {
        //         int ProductId = updateShipment.ProductId;
        //         short FlavourId = updateShipment.FlavourId;
        //         Product product = products.First(e => e.Id == ProductId);
        //         short NewReturnQuantity = updateShipment.TotalRecievedPieces;
        //         OutgoingShipmentDetails CurrentOutgoingDetail = outgoingShipment.OutgoingShipmentDetails.First(e => e.Id == updateShipment.Id);
        //         short CurrentReturnQuantity = CurrentOutgoingDetail.TotalQuantityReturned;

        //         CurrentOutgoingDetail.TotalQuantityReturned = NewReturnQuantity;

        //         short AbsoluteReturnQuantity = (short)Math.Abs(CurrentReturnQuantity - NewReturnQuantity);
        //         short NewSchemeQuantity = Utility.GetTotalSchemeQuantity(CurrentOutgoingDetail.TotalQuantityShiped - updateShipment.TotalRecievedPieces, product.CaretSize, product.SchemeQuantity);
        //         short AbsoluteSchemeQuantity = (short)Math.Abs(CurrentOutgoingDetail.SchemeTotalQuantity - NewSchemeQuantity);

        //         if (NewReturnQuantity > CurrentReturnQuantity)
        //         {
        //             _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, AbsoluteReturnQuantity);
        //             _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, AbsoluteSchemeQuantity);
        //         }
        //         if (NewReturnQuantity < CurrentReturnQuantity)
        //         {
        //             _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, AbsoluteReturnQuantity);
        //             _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, AbsoluteSchemeQuantity);
        //         }
        //         CurrentOutgoingDetail.SchemeTotalPrice = Utility.GetTotalProductPrice(products.First(e => e.Id == SchemeProductDetail.ProductId), NewSchemeQuantity);
        //         _unitOfWork.OutgoingShipmentDetailRepository.Update(CurrentOutgoingDetail);
        //     }
        //     foreach (ShipmentDTO newShipment in recieveShipments.Where(e => e.Id == 0))
        //     {
        //         int ProductId = newShipment.ProductId;
        //         byte FlavourId = newShipment.FlavourId;
        //         Product product = products.First(e => e.Id == ProductId);

        //         OutgoingShipmentDetails outgoingShipmentDetail = outgoingShipment.OutgoingShipmentDetails.First(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId);
        //         short SchemeQuantity = Utility.GetTotalSchemeQuantity(outgoingShipmentDetail.TotalQuantityShiped - newShipment.TotalRecievedPieces, outgoingShipmentDetail.CaretSize, product.SchemeQuantity);
        //         outgoingShipmentDetail.SchemeTotalQuantity = (byte)SchemeQuantity;
        //         outgoingShipmentDetail.SchemeTotalPrice = Utility.GetTotalProductPrice(SchemeProduct, SchemeQuantity);
        //         _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, SchemeQuantity);
        //         _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, newShipment.TotalRecievedPieces);
        //         _unitOfWork.OutgoingShipmentDetailRepository.Update(outgoingShipmentDetail);
        //     }

        //     int[] RecieveOutgoingDetailIds = recieveShipments.Select(e => e.Id).ToArray();//Id of shipment recieve
        //     //loop through shipment that previous have return but not now
        //     foreach (OutgoingShipmentDetails nonExistDetails in outgoingShipment.OutgoingShipmentDetails.Where(e => !RecieveOutgoingDetailIds.Contains(e.Id) && e.TotalQuantityReturned > 0))
        //     {
        //         nonExistDetails.SchemeTotalQuantity = 0;
        //         nonExistDetails.SchemeTotalPrice = 0;
        //         nonExistDetails.TotalQuantityReturned = 0;
        //         _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, nonExistDetails.SchemeTotalQuantity);
        //         _unitOfWork.OutgoingShipmentDetailRepository.Update(nonExistDetails);
        //     }
        //     return await _unitOfWork.SaveChangesAsync() > 0;
        // }
        // public IEnumerable<OutgoingShipmentInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        // {
        //     IEnumerable<OutgoingShipment> outgoings = readOutgoingShipmentRepository.GetShipmentsBySalesmnaIdAndDate(salesmanId, date);
        //     IEnumerable<OutgoingShipmentInfoDTO> result = _mapper.Map<IEnumerable<OutgoingShipmentInfoDTO>>(outgoings);
        //     return result;
        // }


        public OutgoingShipmentInfoDTO GetOutgoingShipmentWithSalesmanInfoDTO(int Id)
        {
            return (_mapper.Map<OutgoingShipmentInfoDTO>(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id)));
        }
        bool IsSchemeQuantityAvailable(IEnumerable<OutgoingShipmentDetailDTO> outgoingShipmentDetailDTOs, IEnumerable<OutgoingShipmentDetails> oldOutoingDetails, IEnumerable<Product> products)
        {
            bool IsValid = true;
            int schemeQuantityLeft = products.First(e => e.Id == this.schemeProductOptions.ProductId).ProductFlavourQuantity.First(e => e.FlavourIdFk == this.schemeProductOptions.FlavourId).Quantity;
            foreach (OutgoingShipmentDetailDTO currentShipmentDetailDTO in outgoingShipmentDetailDTOs)
            {
                OutgoingShipmentDetails oldDetail = oldOutoingDetails.FirstOrDefault(e => e.ProductIdFk == currentShipmentDetailDTO.ProductId && e.FlavourIdFk == currentShipmentDetailDTO.FlavourId);
                short oldSchemeQuantity = oldDetail.SchemeTotalQuantity;
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
        IEnumerable<ProductFlavourElement> CheckSchemeQuantityValid(IEnumerable<OutgoingShipmentDetailDTO> outgoingShipmentDetailDTOs, IEnumerable<Product> products)
        {
            ICollection<ProductFlavourElement> productFlavourElements = new List<ProductFlavourElement>();
            foreach (OutgoingShipmentDetailDTO outgoingDetail in outgoingShipmentDetailDTOs)
            {
                Product product = products.First(e => e.Id == outgoingDetail.Id);
                if (Utility.GetTotalSchemeQuantity(outgoingDetail.TotalQuantityShiped, product.CaretSize, outgoingDetail.SchemeInfo.SchemeQuantity) != outgoingDetail.SchemeInfo.TotalQuantity)
                    productFlavourElements.Add(new ProductFlavourElement { FlavourId = outgoingDetail.FlavourId, ProductId = outgoingDetail.ProductId });
            }
            return productFlavourElements.Count > 0 ? productFlavourElements : null;
        }
        IEnumerable<ProductFlavourElement> CheckQuantityShipedValid(IEnumerable<OutgoingShipmentDetailDTO> outgoingShipmentDetailDTOs)
        {
            ICollection<ProductFlavourElement> productFlavourElements = new List<ProductFlavourElement>();
            foreach (OutgoingShipmentDetailDTO productFlavourElement in outgoingShipmentDetailDTOs)
                if (productFlavourElement.TotalQuantityShiped - productFlavourElement.TotalQuantityReturned != productFlavourElement.TotalQuantityTaken)
                    productFlavourElements.Add(new ProductFlavourElement { ProductId = productFlavourElement.ProductId, FlavourId = productFlavourElement.FlavourId });
            return productFlavourElements.Count > 0 ? productFlavourElements : null;
        }
        public ResultModel Update(OutgoingShipmentDTO outgoingShipmentDTO)
        {
            IEnumerable<ProductFlavourElement> productFlavourElements = CheckQuantityShipedValid(outgoingShipmentDTO.OutgoingShipmentDetails);
            if (productFlavourElements != null)
                return new ResultModel { Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SHIPED_QUANTITY_NOT_VALID), Code = ((int)OutgoingErroCode.SHIPED_QUANTITY_NOT_VALID), Content = productFlavourElements };

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(outgoingShipmentDTO.Id);
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking(outgoingShipment.DateCreated);

            //check Given TotalSchemeQuantity == (QSinCaret)*SchemeQuantityPerCaret
            {
                IEnumerable<ProductFlavourElement> productWithSchemeNotValid = this.CheckSchemeQuantityValid(outgoingShipmentDTO.OutgoingShipmentDetails, products);
                if (productWithSchemeNotValid != null)
                    return new ResultModel { Code = ((int)OutgoingErroCode.SCHEME_QUANTITY_NOT_VALID), Name = System.Enum.GetName(typeof(OutgoingErroCode), OutgoingErroCode.SCHEME_QUANTITY_NOT_VALID), IsValid = false, Content = productWithSchemeNotValid };
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

            IEnumerable<OutgoingShipmentDetails> deleteShipments = outgoingShipment.OutgoingShipmentDetails
            .Where(e => !outgoingShipmentDTO.OutgoingShipmentDetails.Any(f => f.FlavourId == e.FlavourIdFk && f.ProductId == e.ProductIdFk && f.OutgoingShipmentId == e.OutgoingShipmentIdFk));
            foreach (var deleteShipment in _mapper.Map<IEnumerable<OutgoingShipmentDetails>>(deleteShipments))
            {
                _unitOfWork.OutgoingShipmentDetailRepository.Delete(deleteShipment.Id);
                _unitOfWork.ProductRepository.AddQuantity(deleteShipment.ProductIdFk, deleteShipment.FlavourIdFk, deleteShipment.TotalQuantityShiped);
                _unitOfWork.ProductRepository.AddQuantity(SchemeProductId, SchemeFlavourId, deleteShipment.SchemeTotalQuantity);
            }

            IEnumerable<OutgoingShipmentDetailDTO> newShipments = outgoingShipmentDTO.OutgoingShipmentDetails.Where(e => e.Id == 0).ToList();

            foreach (var newShipment in newShipments)
            {
                OutgoingShipmentDetails newShipmentDetails = _mapper.Map<OutgoingShipmentDetails>(newShipment);

                int ProductId = newShipment.ProductId; short FlavourId = newShipment.FlavourId;
                newShipmentDetails.OutgoingShipmentIdFk = outgoingShipment.Id;
                Product product = products.First(e => e.Id == ProductId);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, newShipment.TotalQuantityShiped + newShipment.TotalQuantityRejected);
                short TotalSchemeQuantity = newShipment.SchemeInfo.TotalQuantity;
                //scheme price and quantity
                newShipmentDetails.SchemeTotalQuantity = ((byte)TotalSchemeQuantity);
                newShipmentDetails.SchemeTotalPrice = Utility.GetTotalProductPrice(SchemeProduct, TotalSchemeQuantity);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductId, SchemeFlavourId, TotalSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Add(newShipmentDetails);
            }


            IEnumerable<OutgoingShipmentDetailDTO> updateShipments = outgoingShipmentDTO.OutgoingShipmentDetails
            .Where(e => outgoingShipment.OutgoingShipmentDetails.Any(f => f.FlavourIdFk == e.FlavourId && f.ProductIdFk == e.ProductId));
            foreach (var shipment in updateShipments)
            {
                OutgoingShipmentDetails updateShipment = _mapper.Map<OutgoingShipmentDetails>(shipment);
                int ProductId = updateShipment.ProductIdFk; short FlavourId = updateShipment.FlavourIdFk;
                Product product = products.First(e => e.Id == ProductId);
                OutgoingShipmentDetails previousShipment = outgoingShipment.OutgoingShipmentDetails.First(e => e.Id == updateShipment.Id);
                short previousQuantity = previousShipment.TotalQuantityShiped;
                short newQuantity = updateShipment.TotalQuantityShiped;

                if (newQuantity == previousQuantity && Utility.GetSchemeQuantityPerCaret(previousQuantity, previousShipment.SchemeTotalQuantity, products.First(e => e.Id == shipment.ProductId).CaretSize) == shipment.SchemeInfo.SchemeQuantity)
                    continue;

                updateShipment.TotalQuantityShiped = newQuantity;
                short newSchemeQuantity = Utility.GetTotalSchemeQuantity(newQuantity, product.CaretSize, shipment.SchemeInfo.SchemeQuantity);
                if (newQuantity > previousQuantity)
                {
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, newQuantity - previousQuantity);
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductId, SchemeFlavourId, newSchemeQuantity - previousShipment.SchemeTotalQuantity);
                }
                else
                {
                    _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, previousQuantity - newQuantity);
                    _unitOfWork.ProductRepository.AddQuantity(SchemeProductId, SchemeFlavourId, previousQuantity - newSchemeQuantity);
                }
                updateShipment.SchemeTotalPrice = Utility.GetTotalProductPrice(SchemeProduct, newSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Update(updateShipment);
                _unitOfWork.SaveChanges();
            }
            return new ResultModel { IsValid = true };
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

        public IEnumerable<OutgoingShipmentInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public OutgoingShipmentInfoDTO GetById(int Id)
        {
            return _mapper.Map<OutgoingShipmentInfoDTO>(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id));
        }

        // public OutgoingShipmentPriceDetailDTO GetPriceDetailById(int Id)
        // {
        //     if (Id == 0)
        //         return null;

        //     OutgoingShipment outgoingShipment = null;
        //     using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //     {
        //         outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
        //     }
        //     OutgoingShipmentPriceDetailDTO outgoingShipmentPriceDetail = new OutgoingShipmentPriceDetailDTO
        //     {
        //         Id = outgoingShipment.Id,
        //         ProductDetails = new List<OutgoingShipmentProductDetailDTO>(),
        //         Salesman = _mapper.Map<SalesmanDTO>(outgoingShipment.SalesmanIdFkNavigation)
        //     };
        //     foreach (OutgoingShipmentDetails outgoingShipmentDetail in outgoingShipment.OutgoingShipmentDetails)
        //     {
        //         int ProductId = outgoingShipmentDetail.ProductIdFk;
        //         short FlavourId = outgoingShipmentDetail.FlavourIdFk;
        //         if (outgoingShipmentPriceDetail.ProductDetails.First(e => e.ProductId == ProductId) == null)
        //         {
        //             outgoingShipmentPriceDetail.ProductDetails.Add(new OutgoingShipmentProductDetailDTO
        //             {
        //                 Name = outgoingShipmentDetail.ProductIdFkNavigation.Name,
        //                 ProductId = outgoingShipmentDetail.ProductIdFk,
        //                 OutgoingShipmentFlavourDetails = new List<OutgoingShipmentFlavourDetailDTO>()
        //             });

        //         }
        //         int FlavourQuantity = outgoingShipmentDetail.TotalQuantityShiped;

        //         ICollection<FlavourQuantityVariantDetailDTO> variantDetailDTOs = new List<FlavourQuantityVariantDetailDTO>();

        //         foreach (CustomCaratPrice customCarat in outgoingShipment.CustomCaratPrice.Where(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId))
        //         {
        //             FlavourQuantity -= customCarat.Quantity;
        //             variantDetailDTOs.Add(new FlavourQuantityVariantDetailDTO
        //             {
        //                 PricePerCarat = customCarat.PricePerCarat,
        //                 Quantity = customCarat.Quantity,
        //                 TotalPrice = customCarat.Quantity * (customCarat.PricePerCarat / outgoingShipmentDetail.CaretSize)
        //             });
        //         }
        //         variantDetailDTOs.Add(new FlavourQuantityVariantDetailDTO
        //         {
        //             PricePerCarat = outgoingShipmentDetail.PricePerCarat ?? 0,
        //             Quantity = FlavourQuantity,
        //             TotalPrice = FlavourQuantity * (outgoingShipmentDetail.PricePerCarat ?? 0 / outgoingShipmentDetail.CaretSize)
        //         });

        //         outgoingShipmentPriceDetail.ProductDetails.First(e => e.ProductId == ProductId)
        //         .OutgoingShipmentFlavourDetails.Add(new OutgoingShipmentFlavourDetailDTO()
        //         {
        //             FlavourId = FlavourId,
        //             FlavourQuantityVariantDetails = variantDetailDTOs,
        //             Name = outgoingShipmentDetail.FlavourIdFkNavigation.Title,
        //             SchemeDetail = new FlavourSchemeDetailDTO
        //             {
        //                 PricePerBottle = outgoingShipmentDetail.SchemeTotalPrice / outgoingShipmentDetail.SchemeTotalQuantity,
        //                 Quantity = outgoingShipmentDetail.SchemeTotalQuantity,
        //                 TotalPrice = outgoingShipmentDetail.SchemeTotalPrice
        //             }
        //         });
        //     }
        //     return outgoingShipmentPriceDetail;
        // }

    }
}