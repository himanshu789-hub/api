using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.Models.DTOModel;
using AutoMapper;
using Shambala.Domain;
using Shambala.Core.Contracts.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shambala.Core.Supervisors
{
    using Exception;
    using Models.BLLModel;
    using Helphers;
    using System;
    using Contracts.Repositories;
    public class OutgoingShipmentSupervisor : IOutgoingShipmentSupervisor
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        readonly IReadInvoiceRepository readInvoiceRepository;
        readonly IReadOutgoingShipmentRepository readOutgoingShipmentRepository;
        byte _gstRate = 18;

        public OutgoingShipmentSupervisor(IMapper mapper, IUnitOfWork unitOfWork, IReadOutgoingShipmentRepository shipmentReadRepository, IReadInvoiceRepository readInvoiceRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.readInvoiceRepository = readInvoiceRepository;
            readOutgoingShipmentRepository = shipmentReadRepository;
        }

        public byte GSTRate => _gstRate;
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
        public async Task<OutgoingShipmentWithSalesmanInfoDTO> AddAsync(PostOutgoingShipmentDTO postOutgoingShipmentDTO)
        {
            OutgoingShipment OutgoingShipment = _mapper.Map<OutgoingShipment>(postOutgoingShipmentDTO);
            if (OutgoingShipment.OutgoingShipmentDetails.Distinct().Count() != OutgoingShipment.OutgoingShipmentDetails.Count())
            {
                throw new DuplicateShipmentsException();
            }

            using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.CheckPostShipment(postOutgoingShipmentDTO.Shipments);
                if (productOutOfStockBLLs == null)
                {
                    this.UpdateQuantities(OutgoingShipment.OutgoingShipmentDetails);
                    _unitOfWork.OutgoingShipmentRepository.Add(OutgoingShipment);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    _unitOfWork.Rollback();
                    return null;
                }
            }
            _unitOfWork.OutgoingShipmentRepository.Load(OutgoingShipment, entity => entity.SalesmanIdFkNavigation);
            return _mapper.Map<OutgoingShipmentWithSalesmanInfoDTO>(OutgoingShipment);
        }

        IEnumerable<ProductReturnBLL> GetReturnProductFromShipments(IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails)
        {
            ICollection<ProductReturnBLL> productReturnBLLs = new List<ProductReturnBLL>();

            foreach (var OutgoingShipmentDetail in outgoingShipmentDetails)
            {
                if (OutgoingShipmentDetail.TotalQuantityReturned >= 0)
                    productReturnBLLs.Add(new ProductReturnBLL
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

        public async Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail)
        {
            int OutgoingShipmentId = shipmentLedgerDetail.Id;
            if (!_unitOfWork.OutgoingShipmentRepository.CheckStatusWithNoTracking(OutgoingShipmentId, Helphers.OutgoingShipmentStatus.RETURN))
                throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.RETURN);

            IEnumerable<ShopCreditOrDebitDTO> shopRecievedCredits = _mapper.Map<IEnumerable<ShopCreditOrDebitDTO>>(shipmentLedgerDetail.Ledgers);

            OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(OutgoingShipmentId);


            IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails = outgoing.OutgoingShipmentDetails;

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            // if (!this.ClearCredit(shipmentLedgerDetail))
            // {
            //     return false;
            // }
            _unitOfWork.OutgoingShipmentRepository.Complete(OutgoingShipmentId);
            await _unitOfWork.SaveChangesAsync();
            return true;

        }

        public OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int orderId)
        {
            IEnumerable<OutgoingShipmentProductInfoDTO> OutgoingShipmentDettailInfos = readOutgoingShipmentRepository.GetProductsById(orderId: orderId);
            OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(orderId);

            OutgoingShipmentWithProductListDTO outgoingShipmentWithProductListDTO = new OutgoingShipmentWithProductListDTO()
            {
                Id = outgoing.Id,
                DateCreated = outgoing.DateCreated,
                Status = (OutgoingShipmentStatus)System.Enum.Parse(typeof(OutgoingShipmentStatus), outgoing.Status),
                Salesman = _mapper.Map<SalesmanDTO>(outgoing.SalesmanIdFkNavigation)
            };
            if (OutgoingShipmentDettailInfos.Count() == 0)
            {
                outgoingShipmentWithProductListDTO.Products = new List<ProductDTO>();
                return outgoingShipmentWithProductListDTO;
            }
            IEnumerable<ProductDTO> Products = OutgoingShipmentDettailInfos.GroupBy(e => e.Product.Id).First()
            .GroupJoin(OutgoingShipmentDettailInfos, e => e.Product.Id, f => f.Product.Id, (e, f) => new ProductDTO()
            {
                CaretSize = e.Product.CaretSize,
                Id = e.Product.Id,
                Name = e.Product.Name,
                Flavours = f.Select(s => s.Flavour).ToList()
            });
            outgoingShipmentWithProductListDTO.Products = Products;
            return outgoingShipmentWithProductListDTO;
        }
        public IEnumerable<ProductOutOfStockBLL> CheckReturnShipment(int Id, IEnumerable<ShipmentDTO> shipments)
        {
            if (_unitOfWork.CurrentTransaction == null)
            {
                using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    return this.GetOverReturnShipment(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id), shipments);
                }
            }
            return this.GetOverReturnShipment(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id), shipments);
        }
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

        public async Task<bool> ReturnShipmentAsync(int Id, IEnumerable<ShipmentDTO> recieveShipments)
        {

            if (IsShipmentsUnique(recieveShipments))
                throw new DuplicateShipmentsException();

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
            IEnumerable<ProductOutOfStockBLL> productReturnOverShipeds = this.GetOverReturnShipment(outgoingShipment, recieveShipments);
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking(outgoingShipment.DateCreated);
            Product SchemeProduct = products.First(e => e.Id == SchemeProductDetail.ProductId);

            if (productReturnOverShipeds != null)
                throw new ShipmentReturnQuantityExceedException();
            if (!this.IsSchemeQuantityAvailable(_mapper.Map<IEnumerable<ShipmentDTO>>(
                outgoingShipment.OutgoingShipmentDetails),
                recieveShipments, SchemeProduct.ProductFlavourQuantity.First(e => e.FlavourIdFk == SchemeProductDetail.FlavourId).Quantity + outgoingShipment.OutgoingShipmentDetails.Sum(e => e.SchemeTotalQuantity), products))
            {
                throw new SchemeProductQuantityExceedException();
            }
            foreach (ShipmentDTO updateShipment in recieveShipments.Where(e => e.Id != 0))
            {
                int ProductId = updateShipment.ProductId;
                short FlavourId = updateShipment.FlavourId;
                Product product = products.First(e => e.Id == ProductId);
                short NewReturnQuantity = updateShipment.TotalRecievedPieces;
                OutgoingShipmentDetails CurrentOutgoingDetail = outgoingShipment.OutgoingShipmentDetails.First(e => e.Id == updateShipment.Id);
                short CurrentReturnQuantity = CurrentOutgoingDetail.TotalQuantityReturned;

                CurrentOutgoingDetail.TotalQuantityReturned = NewReturnQuantity;

                short AbsoluteReturnQuantity = (short)Math.Abs(CurrentReturnQuantity - NewReturnQuantity);
                short NewSchemeQuantity = Utility.GetTotalSchemeQuantity(CurrentOutgoingDetail.TotalQuantityShiped - updateShipment.TotalRecievedPieces, product.CaretSize, product.SchemeQuantity);
                short AbsoluteSchemeQuantity = (short)Math.Abs(CurrentOutgoingDetail.SchemeTotalQuantity - NewSchemeQuantity);

                if (NewReturnQuantity > CurrentReturnQuantity)
                {
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, AbsoluteReturnQuantity);
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, AbsoluteSchemeQuantity);
                }
                if (NewReturnQuantity < CurrentReturnQuantity)
                {
                    _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, AbsoluteReturnQuantity);
                    _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, AbsoluteSchemeQuantity);
                }
                CurrentOutgoingDetail.SchemeTotalPrice = Utility.GetTotalSchemePrice(products.First(e=>e.Id==SchemeProductDetail.ProductId),NewSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Update(CurrentOutgoingDetail);
            }
            foreach (ShipmentDTO newShipment in recieveShipments.Where(e => e.Id == 0))
            {

            }
            int[] RecieveOutgoingDetailIds = recieveShipments.Select(e => e.Id).ToArray();
            foreach (OutgoingShipmentDetails deleteDetails in outgoingShipment.OutgoingShipmentDetails.Where(e => !RecieveOutgoingDetailIds.Contains(e.Id)))
            {
                _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, deleteDetails.SchemeTotalQuantity);
                _unitOfWork.ProductRepository.AddQuantity(deleteDetails.ProductIdFk, deleteDetails.FlavourIdFk, deleteDetails.TotalQuantityShiped);
                _unitOfWork.OutgoingShipmentDetailRepository.Delete(deleteDetails.Id);
            }
            // if (!_unitOfWork.OutgoingShipmentRepository.CheckStatusWithNoTracking(Id, Helphers.OutgoingShipmentStatus.PENDING))
            //     throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.PENDING);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        public IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            IEnumerable<OutgoingShipment> outgoings = readOutgoingShipmentRepository.GetShipmentsBySalesmnaIdAndDate(salesmanId, date);
            IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> result = _mapper.Map<IEnumerable<OutgoingShipmentWithSalesmanInfoDTO>>(outgoings);
            return result;
        }

        decimal CalculateTotalAmountOfProduct(int caretSize, decimal pricePerCaret, int quantity)
        {
            return (pricePerCaret / caretSize) * quantity;
        }

        public OutgoingShipmentWithSalesmanInfoDTO GetOutgoingShipmentWithSalesmanInfoDTO(int Id)
        {
            return (_mapper.Map<OutgoingShipmentWithSalesmanInfoDTO>(_unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id)));
        }
        bool IsSchemeQuantityAvailable(IEnumerable<ShipmentDTO> outgoingShipmentDetails, IEnumerable<ShipmentDTO> returnShipments, int quantity, IEnumerable<Product> products)
        {
            bool IsValid = true;
            foreach (ShipmentDTO outgoingShipmentDetail in outgoingShipmentDetails)
            {
                ShipmentDTO returnShipment = returnShipments.FirstOrDefault(e => e.ProductId == outgoingShipmentDetail.ProductId && e.FlavourId == outgoingShipmentDetail.FlavourId);
                Product product = products.First(e => e.Id == outgoingShipmentDetail.ProductId);
                int NewQuantity = Utility.GetTotalSchemeQuantity(outgoingShipmentDetail.TotalRecievedPieces - returnShipment?.TotalRecievedPieces ?? 0, product.CaretSize, products.First(e => e.Id == outgoingShipmentDetail.ProductId).SchemeQuantity);

                if (quantity >= NewQuantity)
                {
                    quantity -= NewQuantity;
                }
                else
                {
                    IsValid = false;
                    break;
                }
            }

            return IsValid;
        }
        public bool Update(int Id, IEnumerable<ShipmentDTO> recieveShipments)
        {

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking(outgoingShipment.DateCreated);
            ProductFlavourQuantity scheme = products.First(e => e.Id == SchemeProductDetail.ProductId).ProductFlavourQuantity.First(e => e.FlavourIdFk == SchemeProductDetail.FlavourId);
            int TotalSchemeProductConsume = outgoingShipment.OutgoingShipmentDetails.Sum(e => e.SchemeTotalQuantity);
            if (!this.IsSchemeQuantityAvailable(recieveShipments,
            Utility.GetReturnShipmentInfoList(outgoingShipment.OutgoingShipmentDetails), scheme.Quantity + TotalSchemeProductConsume, products))
            {
                throw new SchemeProductQuantityExceedException();
            }

            //check quantity available            
            IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.GetOutOfStockList(outgoingShipment, recieveShipments, products);
            if (productOutOfStockBLLs != null)
            {
                throw new QuantityOutOfStockException();
            }

            IEnumerable<ShipmentDTO> currentShipments = _mapper.Map<IEnumerable<ShipmentDTO>>(outgoingShipment.OutgoingShipmentDetails);
            IEnumerable<ShipmentDTO> newShipments = recieveShipments.Except(currentShipments);

            IEnumerable<ShipmentDTO> deleteShipments = currentShipments.Except(recieveShipments);
            foreach (var deleteShipment in _mapper.Map<IEnumerable<OutgoingShipmentDetails>>(deleteShipments))
            {
                _unitOfWork.OutgoingShipmentDetailRepository.Delete(deleteShipment.Id);
                _unitOfWork.ProductRepository.AddQuantity(deleteShipment.ProductIdFk, deleteShipment.FlavourIdFk, deleteShipment.TotalQuantityShiped);
                _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, deleteShipment.SchemeTotalQuantity);
            }

            Product SchemeProduct = products.First(e => e.Id == SchemeProductDetail.ProductId);

            foreach (var newShipment in _mapper.Map<IEnumerable<OutgoingShipmentDetails>>(newShipments))
            {
                int ProductId = newShipment.ProductIdFk; short FlavourId = newShipment.FlavourIdFk;
                newShipment.OutgoingShipmentIdFk = Id;
                Product product = products.First(e => e.Id == ProductId);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, newShipment.TotalQuantityShiped + newShipment.TotalQuantityRejected);
                short TotalSchemeQuantity = Utility.GetSchemeQuantityPerCaret(newShipment.TotalQuantityShiped, product.SchemeQuantity, product.CaretSize);
                //scheme price and quantity
                newShipment.SchemeTotalQuantity = (byte)TotalSchemeQuantity;
                newShipment.SchemeTotalPrice = Utility.GetTotalSchemePrice(SchemeProduct, TotalSchemeQuantity);
                _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, TotalSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Add(newShipment);
            }


            IEnumerable<ShipmentDTO> updateShipments = recieveShipments.Intersect(currentShipments);
            foreach (var updateShipment in _mapper.Map<IEnumerable<OutgoingShipmentDetails>>(updateShipments))
            {
                int ProductId = updateShipment.ProductIdFk; short FlavourId = updateShipment.FlavourIdFk;
                Product product = products.First(e => e.Id == ProductId);
                OutgoingShipmentDetails CurrentShipment = outgoingShipment.OutgoingShipmentDetails.First(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId);
                short CurrentQuantity = CurrentShipment.TotalQuantityShiped;
                short NewQuantity = updateShipment.TotalQuantityShiped;

                if (NewQuantity == CurrentQuantity)
                    continue;

                updateShipment.TotalQuantityShiped = NewQuantity;
                short NewSchemeQuantity = Utility.GetTotalSchemeQuantity(NewQuantity - CurrentShipment.TotalQuantityReturned, product.CaretSize, product.SchemeQuantity);
                if (NewQuantity > CurrentQuantity)
                {
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(ProductId, FlavourId, NewQuantity - CurrentQuantity);
                    _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, NewSchemeQuantity - CurrentShipment.SchemeTotalQuantity);
                }
                else
                {
                    _unitOfWork.ProductRepository.AddQuantity(ProductId, FlavourId, CurrentQuantity - NewQuantity);
                    _unitOfWork.ProductRepository.AddQuantity(SchemeProductDetail.ProductId, SchemeProductDetail.FlavourId, CurrentQuantity - NewSchemeQuantity);

                }
                updateShipment.SchemeTotalPrice = Utility.GetTotalSchemePrice(SchemeProduct, NewSchemeQuantity);
                _unitOfWork.OutgoingShipmentDetailRepository.Update(updateShipment);
                _unitOfWork.SaveChanges();
            }
            return true;
        }
        bool IsShipmentsUnique(IEnumerable<ShipmentDTO> shipments)
        {
            return shipments.Distinct().Count() == shipments.Count();
        }
        public IEnumerable<ProductOutOfStockBLL> CheckPostShipment(IEnumerable<ShipmentDTO> shipments, int? Id = null)
        {
            if (IsShipmentsUnique(shipments))
                throw new DuplicateShipmentsException();
            if (_unitOfWork.CurrentTransaction == null)
            {
                using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking();
                    OutgoingShipment outgoing = Id == null ? null : _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id.Value);
                    return GetOutOfStockList(outgoing, shipments, products);
                }
            }
            IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAllWithNoTracking();
            OutgoingShipment outgoingShipment = Id == null ? null : _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id.Value);
            return GetOutOfStockList(outgoingShipment, shipments, productList);
        }
        IEnumerable<ProductOutOfStockBLL> GetOutOfStockList(OutgoingShipment outgoingShipment, IEnumerable<ShipmentDTO> recieveShipments, IEnumerable<Product> products)
        {
            ICollection<ProductOutOfStockBLL> productOutOfStocks = new List<ProductOutOfStockBLL>();

            foreach (ShipmentDTO shipment in recieveShipments)
            {
                int ProductId = shipment.ProductId;
                short FlavourId = shipment.FlavourId;
                int ProductQuantity = products.First(e => e.Id == ProductId).ProductFlavourQuantity.Where(e => e.FlavourIdFk == FlavourId).First().Quantity;
                if (outgoingShipment != null)
                {
                    ProductQuantity += outgoingShipment.OutgoingShipmentDetails.FirstOrDefault(e => e.FlavourIdFk == FlavourId && e.ProductIdFk == ProductId)?.TotalQuantityShiped ?? 0;
                }
                if (shipment.TotalRecievedPieces > ProductQuantity)
                {
                    productOutOfStocks.Add(new ProductOutOfStockBLL() { FlavourId = FlavourId, ProductId = ProductId, Quantity = ProductQuantity });
                }
            }
            return productOutOfStocks.Count() > 0 ? productOutOfStocks : null;
        }
        public OutgoingShipmentPriceDetailDTO GetPriceDetailById(int Id)
        {
            if (Id == 0)
                return null;

            OutgoingShipment outgoingShipment = null;
            using (var transaction = _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
            }
            OutgoingShipmentPriceDetailDTO outgoingShipmentPriceDetail = new OutgoingShipmentPriceDetailDTO
            {
                Id = outgoingShipment.Id,
                ProductDetails = new List<OutgoingShipmentProductDetailDTO>(),
                Salesman = _mapper.Map<SalesmanDTO>(outgoingShipment.SalesmanIdFkNavigation)
            };
            foreach (OutgoingShipmentDetails outgoingShipmentDetail in outgoingShipment.OutgoingShipmentDetails)
            {
                int ProductId = outgoingShipmentDetail.ProductIdFk;
                short FlavourId = outgoingShipmentDetail.FlavourIdFk;
                if (outgoingShipmentPriceDetail.ProductDetails.First(e => e.ProductId == ProductId) == null)
                {
                    outgoingShipmentPriceDetail.ProductDetails.Add(new OutgoingShipmentProductDetailDTO
                    {
                        Name = outgoingShipmentDetail.ProductIdFkNavigation.Name,
                        ProductId = outgoingShipmentDetail.ProductIdFk,
                        OutgoingShipmentFlavourDetails = new List<OutgoingShipmentFlavourDetailDTO>()
                    });

                }
                int FlavourQuantity = outgoingShipmentDetail.TotalQuantityShiped;

                ICollection<FlavourQuantityVariantDetailDTO> variantDetailDTOs = new List<FlavourQuantityVariantDetailDTO>();

                foreach (CustomCaratPrice customCarat in outgoingShipment.CustomCaratPrice.Where(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId))
                {
                    FlavourQuantity -= customCarat.Quantity;
                    variantDetailDTOs.Add(new FlavourQuantityVariantDetailDTO
                    {
                        PricePerCarat = customCarat.PricePerCarat,
                        Quantity = customCarat.Quantity,
                        TotalPrice = customCarat.Quantity * (customCarat.PricePerCarat / outgoingShipmentDetail.CaretSize)
                    });
                }
                variantDetailDTOs.Add(new FlavourQuantityVariantDetailDTO
                {
                    PricePerCarat = outgoingShipmentDetail.PricePerCarat ?? 0,
                    Quantity = FlavourQuantity,
                    TotalPrice = FlavourQuantity * (outgoingShipmentDetail.PricePerCarat ?? 0 / outgoingShipmentDetail.CaretSize)
                });

                outgoingShipmentPriceDetail.ProductDetails.First(e => e.ProductId == ProductId)
                .OutgoingShipmentFlavourDetails.Add(new OutgoingShipmentFlavourDetailDTO()
                {
                    FlavourId = FlavourId,
                    FlavourQuantityVariantDetails = variantDetailDTOs,
                    Name = outgoingShipmentDetail.FlavourIdFkNavigation.Title,
                    SchemeDetail = new FlavourSchemeDetailDTO
                    {
                        PricePerBottle = outgoingShipmentDetail.SchemeTotalPrice / outgoingShipmentDetail.SchemeTotalQuantity,
                        Quantity = outgoingShipmentDetail.SchemeTotalQuantity,
                        TotalPrice = outgoingShipmentDetail.SchemeTotalPrice
                    }
                });
            }
            return outgoingShipmentPriceDetail;
        }

    }
}