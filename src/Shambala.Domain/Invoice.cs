using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Invoice
    {
        public int Id { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public short ShopIdFk { get; set; }
        public short? SchemeIdFk { get; set; }
        public System.DateTime DateCreated{get;set;}
        public byte Gstrate { get; set; }
        public decimal Price { get; set; }
        public bool IsCleared{get;set;}
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        public virtual Scheme SchemeIdFkNavigation { get; set; }
        public virtual Shop ShopIdFkNavigation { get; set; }
    }
}
