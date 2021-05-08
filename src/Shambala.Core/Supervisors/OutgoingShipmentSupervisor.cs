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
    public class OutgoingShipmentSupervisor : IOutgoingShipmentSupervisor
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
        byte _gstRate = 18;

        public OutgoingShipmentSupervisor(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public byte GSTRate => _gstRate;

        public async Task<OutgoingShipmentWithSalesmanInfoDTO> AddAsync(PostOutgoingShipmentDTO postOutgoingShipmentDTO)
        {
            OutgoingShipment OutgoingShipment = _mapper.Map<OutgoingShipment>(postOutgoingShipmentDTO);
            if (OutgoingShipment.OutgoingShipmentDetails.Distinct().Count() != OutgoingShipment.OutgoingShipmentDetails.Count())
            {
                throw new DuplicateShipmentsException();
            }

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            _unitOfWork.OutgoingShipmentRepository.Add(OutgoingShipment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OutgoingShipmentWithSalesmanInfoDTO>(OutgoingShipment);
        }

        IEnumerable<ProductReturnBLL> GetProductReturnFromShipments(IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails, IEnumerable<InvoiceDTO> invoiceDTOs)
        {
            ICollection<ProductReturnBLL> productReturnBLLs = new List<ProductReturnBLL>();
            foreach (var OutgoingShipmentDetail in outgoingShipmentDetails)
            {
                int ProductId = OutgoingShipmentDetail.ProductIdFk;
                int FlavourId = OutgoingShipmentDetail.FlavourIdFk;
                IEnumerable<InvoiceDTO> InvoicesWithParticularProductFlavour = invoiceDTOs.Where(e => e.ProductId == ProductId && e.FlavourId == FlavourId);
                int QuantityToReturn = InvoicesWithParticularProductFlavour.Sum(e => e.Quantity);
                productReturnBLLs.Add(new ProductReturnBLL
                {
                    ProductId = ProductId,
                    FlavourId = FlavourId,
                    Quantity = QuantityToReturn
                });
            }
            return productReturnBLLs;
        }

        void AddInvoice(IEnumerable<InvoiceDTO> invoiceDTOs)
        {
            IEnumerable<Invoice> Invoices = _mapper.Map<IEnumerable<Invoice>>(invoiceDTOs);
            IEnumerable<Product> Products = _unitOfWork.ProductRepository.GetAllWithNoTracking();

            foreach (Invoice invoice in Invoices)
            {
                ApplySchemeOverInvoice(invoice, Products);
                _unitOfWork.InvoiceRepository.Add(invoice);
            }
        }
        void ApplySchemeOverInvoice(Invoice invoice, IEnumerable<Product> products)
        {
            short? SchemeId = invoice.SchemeIdFk;
            if (!SchemeId.HasValue)
                return;
            Scheme scheme = _unitOfWork.SchemeRepository.GetSchemeWithNoTrackingById(SchemeId.Value);
            byte InvoiceSchemeType = scheme.SchemeType;

            Product Product = products.FirstOrDefault(e => e.Id == invoice.ProductIdFk);
            decimal Price = Product.PricePerCaret * invoice.QuantityPurchase;
            // add Cost Price
            invoice.CostPrice = Price;

            invoice.SellingPrice = invoice.CostPrice;
            if (InvoiceSchemeType == (byte)SchemeType.Percentage)
            {
                invoice.SellingPrice *= (1 - scheme.Value);
                return;
            }
            int QuantityToDeduct = (int)(InvoiceSchemeType == (byte)SchemeType.Bottle ? (scheme.Value) : (scheme.Value * Product.CaretSize));
            _unitOfWork.ProductRepository.DeductQuantityOfProductFlavour(invoice.ProductIdFk, invoice.FlavourIdFk, QuantityToDeduct);
        }
        public async Task<bool> CompleteAsync(int OutgoingShipmentId, IEnumerable<InvoiceDTO> invoiceDTOs)
        {
            if (_unitOfWork.OutgoingShipmentRepository.CheckStatus(OutgoingShipmentId, Helphers.OutgoingShipmentStatus.RETURN))
                throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.RETURN);

            OutgoingShipment outgoing = _unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(OutgoingShipmentId);
            IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails = outgoing.OutgoingShipmentDetails;
            IEnumerable<ProductReturnBLL> productReturnBLLs = this.GetProductReturnFromShipments(outgoingShipmentDetails, invoiceDTOs);

            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            this.AddInvoice(invoiceDTOs);
            _unitOfWork.ProductRepository.ReturnQuantity(productReturnBLLs);
            _unitOfWork.OutgoingShipmentRepository.Complete(OutgoingShipmentId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public IEnumerable<ProductDTO> GetProductListByOrderId(int orderId)
        {
            IEnumerable<OutgoingShipmentDettailInfo> OutgoingShipmentDettailInfos = _unitOfWork.OutgoingShipmentRepository.GetProductsById(orderId: orderId);
            ICollection<ProductDTO> Products = new List<ProductDTO>();
            foreach (var item in OutgoingShipmentDettailInfos)
            {
                if (Products.FirstOrDefault(e => e.Id == item.Product.Id) != null)
                {
                    var Product = Products.FirstOrDefault(e => e.Id == item.Product.Id);
                    Product.Flavours.Add(item.Flavour);
                }
                else
                {
                    Products.Add(new ProductDTO() { CaretSize = item.Product.CaretSize, Id = item.Product.Id, Name = item.Product.Name });
                }
            }
            return Products;
        }

        public async Task<bool> ReturnAsync(OutgoingShipmentDTO outgoingShipmentDTO)
        {
            OutgoingShipment outgoing = _mapper.Map<OutgoingShipment>(outgoingShipmentDTO);
            var returnShipments = outgoing.OutgoingShipmentDetails;
            if (returnShipments.Distinct().Count() != outgoing.OutgoingShipmentDetails.Count())
                throw new DuplicateShipmentsException();
            if (!_unitOfWork.OutgoingShipmentRepository.CheckStatus(outgoing.Id, Helphers.OutgoingShipmentStatus.PENDING))
                throw new OutgoingShipmentNotOperableException(Helphers.OutgoingShipmentStatus.PENDING);
            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            _unitOfWork.OutgoingShipmentRepository.Return(outgoing.Id, outgoing.OutgoingShipmentDetails);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

    }
}