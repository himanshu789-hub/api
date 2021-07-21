using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Debit
    {
        public int Id { get; set; }
        public DateTime DateRecieved { get; set; }
        public decimal Amount { get; set; }
        public short ShopIdFk { get; set; }
        public int OutgoingShipmentIdFk { get; set; }

        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        public virtual Shop ShopIdFkNavigation { get; set; }
    }
}
