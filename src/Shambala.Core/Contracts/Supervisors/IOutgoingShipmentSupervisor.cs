using Shambala.Core.DTOModels;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface  IOutgoingShipmentSupervisor
    {
        Task<OutgoingShipmentWithSalesmanInfoDTO>  AddAsync(PostOutgoingShipmentDTO outgoingShipment);
        Task<bool> ReturnAsync(OutgoingShipmentDTO outgoingShipmentDTO);
        IEnumerable<ProductDTO> GetProductListByOrderId(int OrderId);

    }
}