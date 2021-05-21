using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Credit
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public short ShopIdFk { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        public virtual Shop ShopIdFkNavigation { get; set; }
    }
}
