using System;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.ComponentModel.DataAnnotations;

namespace Shambala.Core.Models.DTOModel
{
    public abstract class OutgoingShipmentBaseDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        public short RowVersion { get; set; }
        public ICollection<OutgoingShipmentDetailTransferDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentDTO : OutgoingShipmentBaseDTO
    {
        [RequiredWithNonDefault]
        public short SalesmanId { get; set; }
    }
    // public class OutgoingShipmentWithProductListDTO
    // {
    //     public int Id { get; set; }
    //     public OutgoingShipmentStatus Status { get; set; }
    //     public DateTime DateCreated { get; set; }
    //     public IEnumerable<ProductDTO> Products { get; set; }
    //     public SalesmanDTO Salesman { get; set; }
    // }
    public class OutgoingShipmentInfoDTO : OutgoingShipmentBaseDTO
    {
        public SalesmanDTO Salesman { get; set; }
    }
    public class OutgoingShipmentDetailTransferDTO : OutgoingShipmentDetailBaseDTO
    {
        public SchemeInfo SchemeInfo { get; set; }
        public CustomCaratPriceDetailDTO CustomCaratPrices { get; set; }
    }
    public class OutgoingShipmentDetailDTO : OutgoingShipmentDetailBaseDTO
    {
        public byte SchemeTotalQuantity { get; set; }
        public decimal SchemeTotalPrice { get; set; }
        public ICollection<CustomCaratPriceDTO> CustomCaratPrices { get; set; }
    }
    public abstract class OutgoingShipmentDetailBaseDTO
    {
        public int Id { get; set; }
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        [RequiredWithNonDefault]
        public short FlavourId { get; set; }
        [RequiredWithNonDefault]
        public short TotalQuantityTaken { get; set; }
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        [RequiredWithNonDefault]
        public short TotalQuantityShiped { get; set; }
        public short TotalQuantityRejected { get; set; }
        public short TotalQuantityReturned { get; set; }
        public decimal NetPrice { get; set; }
        public decimal TotalShipedPrice { get; set; }
    }
    public class CustomCaratPriceDetailDTO
    {
        public ICollection<CustomCaratPriceDTO> Prices { get; set; }
        public decimal TotalPrice { get; set; }
        public short TotalQuantity { get; set; }
    }
    public class SchemeInfo
    {

        public byte SchemeQuantity { get; set; }
        public short TotalQuantity { get; set; }
        public decimal TotalSchemePrice { get; set; }
    }
    public class OutgoingShipmentPostDTO
    {
        [RequiredWithNonDefault]
        public short SalesmanId { get; set; }
        [RequiredWithNonDefault]
        public DateTime DateCreated { get; set; }
        public IEnumerable<ShipmentDTO> Shipments { get; set; }
    }

}