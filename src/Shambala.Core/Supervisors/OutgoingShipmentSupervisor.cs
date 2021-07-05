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
        public IEnumerable<ProductOutOfStockBLL> ProvideOutOfStockQuantities(IEnumerable<ShipmentDTO> shipment)
        {
            ICollection<ProductOutOfStockBLL> productOutOfStockBLLs = new List<ProductOutOfStockBLL>();
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking();

            foreach (var item in shipment)
            {
                int QuantityShiped = item.TotalRecievedPieces - item.TotalDefectPieces;
                int ProductId = item.ProductId; int FlavourId = item.FlavourId;
                if (!((products.SelectMany(e => e.ProductFlavourQuantity).Where(e => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId).First().Quantity - item.TotalDefectPieces) >= QuantityShiped))
                    productOutOfStockBLLs.Add(new ProductOutOfStockBLL() { FlavourId = FlavourId, ProductId = ProductId });
            }
            if (productOutOfStockBLLs.Count > 0)
                return productOutOfStockBLLs;

            return null;
        }
        void UpdateQuantities(IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails)
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
                IEnumerable<ProductOutOfStockBLL> productOutOfStockBLLs = this.ProvideOutOfStockQuantities(postOutgoingShipmentDTO.Shipments);
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

        IEnumerable<ProductReturnBLL> GetProductLeftOverFromShipments(IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails)
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
        void ApplySchemeOverInvoice(Invoice invoice, IEnumerable<Product> products)
        {
            //     short? SchemeId = invoice.SchemeIdFk;

            //     Scheme scheme = null;
            //     if (SchemeId != null)
            //         scheme = _unitOfWork.SchemeRepository.GetSchemeWithNoTrackingById(SchemeId.Value);
            //     byte? InvoiceSchemeType = scheme?.SchemeType;
            //     Product Product = products.FirstOrDefault(e => e.Id == invoice.ProductIdFk);
            //     decimal Price = (Product.PricePerCaret / Product.CaretSize) * invoice.QuantityPurchase;
            //     // add Cost Price
            //     invoice.CostPrice = Price;
            //     invoice.CaretSize = Product.CaretSize;
            //     invoice.SellingPrice = invoice.CostPrice;
            //     if (InvoiceSchemeType != null)
            //     {
            //         if (InvoiceSchemeType == (byte)SchemeType.Percentage)
            //         {
            //             invoice.SellingPrice *= (1 - scheme.Value);
            //             return;
            //         }
            //         int QuantityToDeduct = (int)(InvoiceSchemeType == (byte)SchemeType.Bottle ? (scheme.Value) : (scheme.Value * Product.CaretSize));
            //         _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(invoice.ProductIdFk, invoice.FlavourIdFk, QuantityToDeduct);
            //     }
            throw new System.NotImplementedException();
        }
        bool ClearCredit(ShipmentLedgerDetail shipmentLedgerDetail)
        {
            IEnumerable<LedgerWithPastDebitDTO> ledgers = shipmentLedgerDetail.Ledgers;
            IEnumerable<ShopCreditOrDebitDTO> shopRecievedCredits = _mapper.Map<IEnumerable<ShopCreditOrDebitDTO>>(ledgers);
            IEnumerable<InvoiceAggreagateDetailBLL> notClearedInvoice = readInvoiceRepository.GetNotClearedAggregateByShopIds(shopRecievedCredits.Select(e => e.ShopId).ToArray());
            IEnumerable<ShopCreditOrDebitDTO> creditLeftOverDTO = Utility.CheckDebitUnderGivenBalance(shopRecievedCredits, notClearedInvoice);
            bool IsAllOk = creditLeftOverDTO.Count() == 0;
            if (IsAllOk)
            {
                foreach (var ledger in ledgers)
                {
                    decimal totalRecievedDue = ledger.OldDebit;
                    foreach (var invoice in notClearedInvoice.Where(e => e.ShopId == ledger.ShopId).OrderBy(e => e.Id))
                    {
                        if (totalRecievedDue > 0)
                        {
                            decimal totalDueLeft = invoice.TotalPrice - invoice.TotalDueCleared;
                            decimal amountToCleared = totalDueLeft >= totalRecievedDue ? totalRecievedDue : totalDueLeft;
                            _unitOfWork.DebitRepository.Add(shipmentLedgerDetail.Id, ledger.ShopId, amountToCleared, shipmentLedgerDetail.DateCreated);
                            totalRecievedDue -= amountToCleared;
                            if (Shambala.Helpher.InvoiceTolerance.IsCleared(invoice.TotalPrice, invoice.TotalDueCleared + amountToCleared))
                            {
                                _unitOfWork.InvoiceRepository.MakeCompleted(invoice.Id);
                            }
                        }
                    }
                }
            }
            return IsAllOk;
        }

        public async Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail)
        {
            int OutgoingShipmentId = shipmentLedgerDetail.Id;
            if (!_unitOfWork.OutgoingShipmentRepository.CheckStatusWithNoTracking(OutgoingShipmentId, Helphers.OutgoingShipmentStatus.RETURN))
                throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.RETURN);

            IEnumerable<ShopCreditOrDebitDTO> shopRecievedCredits = _mapper.Map<IEnumerable<ShopCreditOrDebitDTO>>(shipmentLedgerDetail.Ledgers);

            OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(OutgoingShipmentId);


            IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails = outgoing.OutgoingShipmentDetails;

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            if (!this.ClearCredit(shipmentLedgerDetail))
            {
                return false;
            }
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
        public async Task ReturnAsync(int Id, IEnumerable<OutgoingShipmentDetailReturnDTO> shipments)
        {
            var returnShipments = _mapper.Map<IEnumerable<OutgoingShipmentDetail>>(shipments);
            foreach (var item in returnShipments)
                item.OutgoingShipmentIdFk = Id;

            if (returnShipments.Distinct().Count() != returnShipments.Count())
                throw new DuplicateShipmentsException();
            IEnumerable<ProductReturnBLL> productReturnBLLs = this.GetProductLeftOverFromShipments(returnShipments);

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            if (!_unitOfWork.OutgoingShipmentRepository.CheckStatusWithNoTracking(Id, Helphers.OutgoingShipmentStatus.PENDING))
                throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.PENDING);
            if (productReturnBLLs.Count() > 0)
                _unitOfWork.ProductRepository.ReturnQuantity(productReturnBLLs);
            _unitOfWork.OutgoingShipmentRepository.Return(Id, returnShipments);
            await _unitOfWork.SaveChangesAsync();
        }
        public IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            IEnumerable<OutgoingShipment> outgoings = readOutgoingShipmentRepository.GetShipmentsBySalesmnaIdAndDate(salesmanId, date);
            IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> result = _mapper.Map<IEnumerable<OutgoingShipmentWithSalesmanInfoDTO>>(outgoings);
            return result;
        }

        public LedgerStatus CheckShipmentAmountById(IEnumerable<LedgerDTO> ledgerDTOs, int Id)
        {
            OutgoingShipment outgoingShipment = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(Id);
            decimal totalShipmentPrice = 0;
            decimal givenLedgerTotal = 0;
            foreach (var ledger in ledgerDTOs)
                givenLedgerTotal += (ledger.Credit + ledger.Debit);

            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAllWithNoTracking();
            foreach (var shipmentDetail in outgoingShipment.OutgoingShipmentDetails)
            {
                Product product = products.First(e => e.Id == shipmentDetail.ProductIdFk);
                int quantityToCalculate = shipmentDetail.TotalQuantityShiped - shipmentDetail.TotalQuantityReturned;
                totalShipmentPrice += CalculateTotalAmountOfProduct(product.CaretSize, product.PricePerCaret, quantityToCalculate);
            }
            return new LedgerStatus() { Result = givenLedgerTotal == totalShipmentPrice, TotalShipmentPrice = totalShipmentPrice, YourAmount = givenLedgerTotal };
        }
        decimal CalculateTotalAmountOfProduct(int caretSize, decimal pricePerCaret, int quantity)
        {
            return (pricePerCaret / caretSize) * quantity;
        }
    }
}