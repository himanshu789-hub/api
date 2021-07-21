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
        bool Update(int Id,IEnumerable<ShipmentDTO> shipments);
        Task<OutgoingShipmentWithSalesmanInfoDTO>  AddAsync(PostOutgoingShipmentDTO outgoingShipment);
        Task<bool> ReturnShipmentAsync(int Id,IEnumerable<ShipmentDTO> shipmentDTOs);
        IEnumerable<ProductOutOfStockBLL> CheckReturnShipment(int Id,IEnumerable<ShipmentDTO> shipments);
        IEnumerable<ProductOutOfStockBLL> CheckPostShipment(IEnumerable<ShipmentDTO> shipmentDTOs,int? Id=null);
        OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int OrderId);
        Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail);
        IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId,System.DateTime date);
        OutgoingShipmentWithSalesmanInfoDTO GetOutgoingShipmentWithSalesmanInfoDTO(int Id);
        OutgoingShipmentPriceDetailDTO GetPriceDetailById(int Id); 
    }
}