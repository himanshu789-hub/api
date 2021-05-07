using System;
using System.Collections.Generic;
using Shambala.Core.Helphers;
namespace Shambala.Core.DTOModels
{
    public abstract class OutgoingShipmentBaseDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        public ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentDTO : OutgoingShipmentBaseDTO
    {
        public int SalesmanId { get; set; }
    }
    public class OutgoingShipmentWithSalesmanInfoDTO:OutgoingShipmentBaseDTO
    {
        public SalesmanDTO Salesman { get; set; }
    }
    public class OutgoingShipmentDetailDTO : ShipmentDTO
    {
        public int OutgoingShipmentId { get; set; }
    }
    public class PostOutgoingShipmentDTO
    {
        public DateTime DateCreated { get; set; }
        public int SalesmanId { get; set; }
        public IEnumerable<ShipmentDTO> Shipments { get; set; }
    }
}