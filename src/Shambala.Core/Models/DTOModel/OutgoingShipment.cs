using System;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.ComponentModel.DataAnnotations;

namespace Shambala.Core.Models.DTOModel
{
    public abstract class OutgoingShipmentInfoBaseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        [Required]
        public ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentInfoDTO : OutgoingShipmentInfoBaseDTO
    {
        public short SalesmanId { get; set; }
    }
    public class OutgoingShipmentWithSalesmanInfoDTO : OutgoingShipmentInfoBaseDTO
    {
        public SalesmanDTO Salesman { get; set; }
    }
    public class OutgoingShipmentDetailDTO : ShipmentDTO
    {
        [Required]
        public int OutgoingShipmentId { get; set; }
    }
    public class OutgoingShipmentPostBaseDTO
    {
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public IEnumerable<ShipmentDTO> Shipments { get; set; }
    }
    public class PostOutgoingShipmentDTO : OutgoingShipmentPostBaseDTO
    {
        [Required]
        public short SalesmanId { get; set; }
    }
}