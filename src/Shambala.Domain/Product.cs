using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Product
    {
        public Product()
        {
            IncomingShipment = new HashSet<IncomingShipment>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetails>();
            ProductFlavourQuantity = new HashSet<ProductFlavourQuantity>();
            Scheme = new HashSet<Scheme>();
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public byte CaretSize { get; set; }
        public decimal PricePerCaret { get; set; }
        public byte? SchemeQuantity { get; set; }

        public virtual ICollection<IncomingShipment> IncomingShipment { get; set; }
        public virtual ICollection<OutgoingShipmentDetails> OutgoingShipmentDetails { get; set; }
        public virtual ICollection<ProductFlavourQuantity> ProductFlavourQuantity { get; set; }
        public virtual ICollection<Scheme> Scheme { get; set; }
    }
}
