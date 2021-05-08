using Shambala.Core.Models.DTOModel;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface IGSTRate
    {
        byte  GSTRate{get;}
    }
    public interface  IOutgoingShipmentSupervisor:IGSTRate
    {
        
        Task<OutgoingShipmentWithSalesmanInfoDTO>  AddAsync(PostOutgoingShipmentDTO outgoingShipment);
        Task<bool> ReturnAsync(OutgoingShipmentDTO outgoingShipmentDTO);
        IEnumerable<ProductDTO> GetProductListByOrderId(int OrderId);
        Task<bool> CompleteAsync(int OutgoingShipmentId,IEnumerable<InvoiceDTO> invoiceDTOs);
    }
}