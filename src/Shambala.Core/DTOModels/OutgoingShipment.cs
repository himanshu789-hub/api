using System;
using System.Collections.Generic;

namespace Shambala.Core.DTOModels
{
    public class OutgoingShipmentDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public SalesmanDTO Salesman { get; set; }
        public ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentDetailDTO : ShipmentDTO
    {
        public int OutgoingShipmentId { get; set; }
    }
    public class PostOutgoingShipmentDTO 
    {
       public DateTime DateCreated{get;set;}
       public int SalesmanId{get;set;}
       public IEnumerable<ShipmentDTO> Shipments{get;set;}
    }
}