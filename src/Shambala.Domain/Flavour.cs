using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Flavour
    {
        public Flavour()
        {
            CustomCaratPrice = new HashSet<CustomCaratPrice>();
            IncomingShipment = new HashSet<IncomingShipment>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetails>();
            ProductFlavourQuantity = new HashSet<ProductFlavourQuantity>();
        }

        public byte Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<CustomCaratPrice> CustomCaratPrice { get; set; }
        public virtual ICollection<IncomingShipment> IncomingShipment { get; set; }
        public virtual ICollection<OutgoingShipmentDetails> OutgoingShipmentDetails { get; set; }
        public virtual ICollection<ProductFlavourQuantity> ProductFlavourQuantity { get; set; }
    }
}
