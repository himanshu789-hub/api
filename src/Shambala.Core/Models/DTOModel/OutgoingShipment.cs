using System;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.ComponentModel.DataAnnotations;

namespace Shambala.Core.Models.DTOModel
{
    public abstract class OutgoingShipmentInfoBaseDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        [Required]
        public ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentWithProductListDTO
    {
        public int Id { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<ProductDTO> Products { get; set; }
        public SalesmanDTO Salesman { get; set; }
    }
    public class OutgoingShipmentInfoDTO : OutgoingShipmentInfoBaseDTO
    {

        public short SalesmanId { get; set; }
    }
    public class OutgoingShipmentWithSalesmanInfoDTO : OutgoingShipmentInfoBaseDTO
    {
        [RequiredWithNonDefault]
        public SalesmanDTO Salesman { get; set; }
    }
    public class OutgoingShipmentDetailDTO : ShipmentDTO
    {
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        public int TotalQuantityReturned { get; set; }
    }
    public class OutgoingShipmentDetailReturnDTO
    {
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        [RequiredWithNonDefault]
        public byte FlavourId { get; set; }
        public int TotalQuantityReturned{get;set;}
        public int TotalQuantityDefected{get;set;} 
    }
    public class OutgoingShipmentPostBaseDTO
    {
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        [RequiredWithNonDefault]
        public IEnumerable<ShipmentDTO> Shipments { get; set; }
    }
    public class PostOutgoingShipmentDTO : OutgoingShipmentPostBaseDTO
    {
        [RequiredWithNonDefault]
        public short SalesmanId { get; set; }
    }
}