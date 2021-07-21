using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class CustomCaratPrice
    {
        public int Id { get; set; }
        public int ProductIdFk { get; set; }
        public byte FlavourIdFk { get; set; }
        public short Quantity { get; set; }
        public decimal PricePerCarat { get; set; }
        public int OutgoinShipmentIdFk { get; set; }

        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual OutgoingShipment OutgoinShipmentIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
    }
}
