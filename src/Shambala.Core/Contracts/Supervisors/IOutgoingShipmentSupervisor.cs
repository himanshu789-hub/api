using System.Collections.Generic;

namespace Shambala.Core.Contracts.Supervisors
{
    using Models;
    using Models.DTOModel;
    public interface IGSTRate
    {
        byte GSTRate { get; }
    }
    public interface IOutgoingShipmentSupervisor : IGSTRate
    {
        ResultModel Update(OutgoingShipmentDTO outgoingShipment);
        ResultModel AddAsync(OutgoingShipmentPostDTO outgoingShipment);
        // Task<bool> ReturnShipmentAsync(int Id,IEnumerable<ShipmentDTO> shipmentDTOs);
        // IEnumerable<ProductOutOfStockBLL> CheckReturnShipment(int Id,IEnumerable<ShipmentDTO> shipments);
        // IEnumerable<ProductOutOfStockBLL> CheckPostShipment(IEnumerable<ShipmentDTO> shipmentDTOs,int? Id=null);
        // OutgoingShipmentWithProductListDTO GetWithProductListByOrderId(int OrderId);
        // Task<bool> CompleteAsync(ShipmentLedgerDetail shipmentLedgerDetail);
        IEnumerable<OutgoingShipmentDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, System.DateTime date);
        OutgoingShipmentInfoDTO GetById(int Id);
        //OutgoingShipmentPriceDetailDTO GetPriceDetailById(int Id); 
        ResultModel IsAggreateValid(OutgoingShipmentAggregateDTO aggregateDTO);
    }
}