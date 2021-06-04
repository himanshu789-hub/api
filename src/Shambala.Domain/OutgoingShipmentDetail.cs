using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class OutgoingShipmentDetail : IEquatable<OutgoingShipmentDetail>
    {
        public int Id { get; set; }
        public int ProductIdFk { get; set; }
        public byte CaretSize { get; set; }
        public int TotalQuantityShiped { get; set; }
        public int TotalQuantityRejected { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public int TotalQuantityReturned{get;set;}
        public byte FlavourIdFk { get; set; }
        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }

        public bool Equals(OutgoingShipmentDetail other)
        {
            return this.OutgoingShipmentIdFk == other.OutgoingShipmentIdFk && this.FlavourIdFk == other.FlavourIdFk && this.ProductIdFk == other.ProductIdFk;
        }
    }
}
