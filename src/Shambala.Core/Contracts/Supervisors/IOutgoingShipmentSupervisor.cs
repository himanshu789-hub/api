using Shambala.Core.Models.DTOModel;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    using Domain;
    using Models.BLLModel;
    public interface IGSTRate
    {
        byte  GSTRate{get;}
    }
    public interface  IOutgoingShipmentSupervisor:IGSTRate
    {        
        Task<OutgoingShipmentWithSalesmanInfoDTO>  AddAsync(PostOutgoingShipmentDTO outgoingShipment);
        Task ReturnAsync(int Id,IEnumerable<ShipmentDTO> shipmentDTOs);
        IEnumerable<ProductOutOfStockBLL> ProvideOutOfStockQuantities(IEnumerable<ShipmentDTO> shipmentDTOs);
        OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int OrderId);
        Task<bool> CompleteAsync(int OutgoingShipmentId,IEnumerable<Invoice> invoices);
        IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId,System.DateTime date); 
    }
}