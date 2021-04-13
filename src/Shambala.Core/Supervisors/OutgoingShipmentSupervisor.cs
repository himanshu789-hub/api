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
    public class OutgoingShipmentSupervisor : IOutgoingShipmentSupervisor
    {
        IMapper _mapper;
        IUnitOfWork _unitOfWork;

        public OutgoingShipmentSupervisor(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Add(OutgoingShipmentDTO outgoingShipmentDTO)
        {
            OutgoingShipment OutgoingShipment = _mapper.Map<OutgoingShipment>(outgoingShipmentDTO);
            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            _unitOfWork.OutgoingShipmentRepository.Add(OutgoingShipment);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public IEnumerable<ProductDTO> GetProductListByOrderId(int orderId)
        {
            IEnumerable<OutgoingShipmentDettailInfo> OutgoingShipmentDettailInfos = _unitOfWork.OutgoingShipmentRepository.GetProductById(orderId: orderId);
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
    }
}