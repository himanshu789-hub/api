using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class OutgoingShipmentDetails
    {
        public int Id { get; set; }
        public int ProductIdFk { get; set; }
        public short TotalQuantityShiped { get; set; }
        public short TotalQuantityRejected { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public byte FlavourIdFk { get; set; }
        public short TotalQuantityReturned { get; set; }
        public short SchemeTotalQuantity { get; set; }
        public decimal SchemeTotalPrice { get; set; }
        public short TotalQuantityTaken { get; set; }
        public decimal TotalShipedPrice { get; set; }
        public decimal NetPrice { get; set; }

        public OutgoingShipmentDetails()
        {
            this.CustomCaratPrices = new HashSet<CustomCaratPrice>();
        }
        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
        public  ICollection<CustomCaratPrice> CustomCaratPrices { get; set; }
    }
}
