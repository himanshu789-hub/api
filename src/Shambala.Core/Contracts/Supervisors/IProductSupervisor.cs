using System.Collections.Generic;
using Shambala.Core.DTOModels;
using System.Threading.Tasks;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface IProductSupervisor
    {
        IEnumerable<ProductDTO> GetAll();
         Task<bool> AddAsync(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs);
    }
}