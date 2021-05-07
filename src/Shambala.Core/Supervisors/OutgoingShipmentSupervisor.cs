using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;
using AutoMapper;
using Shambala.Domain;
using Shambala.Core.Contracts.UnitOfWork;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Shambala.Core.Supervisors
{
    using Exception;
    public class OutgoingShipmentSupervisor : IOutgoingShipmentSupervisor
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public OutgoingShipmentSupervisor(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
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