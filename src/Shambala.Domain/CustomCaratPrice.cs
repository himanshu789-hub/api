using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class CustomCaratPrice
    {
        public long Id{get;set;}
        public short Quantity { get; set; }
        public decimal PricePerCarat { get; set; }
        public long OutgoinShipmentDetailIdFk { get; set; } 
        public virtual OutgoingShipmentDetails OutgoinShipmentDetailIdFkNavigation{get;set;}
    }
}
