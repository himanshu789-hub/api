using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Flavour
    {
        public Flavour()
        {
            Invoice = new HashSet<Invoice>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetails>();
            ProductFlavourQuantity = new HashSet<ProductFlavourQuantity>();
        }

        public byte Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<OutgoingShipmentDetails> OutgoingShipmentDetails { get; set; }
        public virtual ICollection<ProductFlavourQuantity> ProductFlavourQuantity { get; set; }
    }
}
