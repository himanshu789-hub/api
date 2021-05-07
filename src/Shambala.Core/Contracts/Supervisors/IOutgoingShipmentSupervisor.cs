using Shambala.Core.DTOModels;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface  IOutgoingShipmentSupervisor
    {
        Task<bool>  AddAsync(PostOutgoingShipmentDTO outgoingShipment);
        IEnumerable<ProductDTO> GetProductListByOrderId(int OrderId);

    }
}