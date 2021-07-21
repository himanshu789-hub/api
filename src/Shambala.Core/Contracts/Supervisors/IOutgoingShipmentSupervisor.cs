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
        Task ReturnAsync(int Id,IEnumerable<OutgoingShipmentDetailReturnDTO> shipmentDTOs);
        IEnumerable<ProductOutOfStockBLL> ProvideOutOfStockQuantities(IEnumerable<ShipmentDTO> shipmentDTOs,IEnumerable<Product> products);
        OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int OrderId);
        Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail);
        IEnumerable<OutgoingShipmentWithSalesmanInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId,System.DateTime date);
        OutgoingShipmentWithSalesmanInfoDTO GetOutgoingShipmentWithSalesmanInfoDTO(int Id);
        IEnumerable<ProductOutOfStockBLL> CheckPostShipment(int? Id,IEnumerable<ShipmentDTO> shipment);
        OutgoingShipmentPriceDetailDTO GetPriceDetailById(int Id); 
    }
}