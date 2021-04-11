using System.Collections.Generic;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;
using Shambala.Core.Contracts.UnitOfWork;
using AutoMapper;
using Shambala.Domain;
namespace Shambala.Core.Supervisors
{
    public class ProductSupervisor : IProductSupervisor
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        public ProductSupervisor(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }
        public async void Add(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs)
        {
            try
            {
                _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
                foreach (var item in incomingShipmentDTOs)
                {
                    IncomingShipment currentShipment = _mapper.Map<IncomingShipment>(item);
                    _unitOfWork.IncomingShipmentRepository.Add(currentShipment);
                    _unitOfWork.ProductRepository.AddQuantity(item.ProductId, item.FlavourId, (short)(item.TotalRecievedPieces - item.TotalDefectPieces));
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                _unitOfWork.Rollback();
                _unitOfWork.Dispose();
            }
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return _mapper.Map<List<ProductDTO>>(_unitOfWork.ProductRepository.GetAll());
        }
    }
}