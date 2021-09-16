using System.Collections.Generic;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
using Shambala.Core.Contracts.UnitOfWork;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Shambala.Domain;
namespace Shambala.Core.Supervisors
{
    public class ProductSupervisor : IProductSupervisor
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;
        ILogger<ProductSupervisor> _logger;
        public ProductSupervisor(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductSupervisor> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<bool> AddAsync(IEnumerable<ShipmentDTO> incomingShipmentDTOs)
        {
            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            foreach (ShipmentDTO item in incomingShipmentDTOs)
            {
                IncomingShipment currentShipment = _mapper.Map<IncomingShipment>(item);
                _unitOfWork.IncomingShipmentRepository.Add(currentShipment);
                _unitOfWork.ProductRepository.AddQuantity(item.ProductId, item.FlavourId, (item.TotalRecievedPieces - item.TotalDefectPieces));
            }
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


        public IEnumerable<ProductDTO> GetAll(System.DateTime? date)
        {
            return _mapper.Map<IEnumerable<ProductDTO>>(_unitOfWork.ProductRepository.GetAllWithNoTracking(date));
        }
        public ProductInfoDTO GetProductsByLeftQuantityAndDispatch(int ProductId, byte? FlavourId)
        {
            _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            return _unitOfWork.ProductRepository.GetProductsInStockWithDispatchQuantity(ProductId, FlavourId);

        }

    }
}