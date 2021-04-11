using System.Collections.Generic;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;
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
        public async void Add(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs)
        {
            try
            {
                _unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
                foreach (IncomingShipmentDTO item in incomingShipmentDTOs)
                {
                    IncomingShipment currentShipment = _mapper.Map<IncomingShipment>(item);
                    _unitOfWork.IncomingShipmentRepository.Add(currentShipment);
                    _unitOfWork.ProductRepository.AddQuantity(item.ProductId, item.FlavourId, (short)(item.TotalRecievedPieces - item.TotalDefectPieces));
                }
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation("Added Incoming Shipment");
            }
            catch (System.Exception e)
            {
                _logger.LogError(e.ToString());
                _unitOfWork.Rollback();
            }
        }

        public IEnumerable<ProductDTO> GetAll()
        {
            return _mapper.Map<List<ProductDTO>>(_unitOfWork.ProductRepository.GetAll());
        }
    }
}