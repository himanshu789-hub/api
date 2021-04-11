using System.Collections.Generic;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface IProductSupervisor
    {
        IEnumerable<ProductDTO> GetAll();
        void Add(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs);
    }
}