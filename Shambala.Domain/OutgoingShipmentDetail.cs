using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class OutgoingShipmentDetail
    {
        public int Id { get; set; }
        public int ProductIdFk { get; set; }
        public byte CaretSize { get; set; }
        public short TotalQuantityShiped { get; set; }
        public byte TotalQuantityRejected { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public byte FlavourIdFk { get; set; }

        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        
    }
}
