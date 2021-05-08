using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Invoice
    {
        public int Id { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public short ShopIdFk { get; set; }
        public int ProductIdFk { get; set; }
        public short FlavourIdFk { get; set; }
        public short? SchemeIdFk { get; set; }
        public byte CaretSize { get; set; }
        public short QuantityPurchase { get; set; }
        
        public byte QuantityDefected { get; set; }
        public byte Gstrate { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }

        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual OutgoingShipment OutgoingShipmentIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
        public virtual Scheme SchemeIdFkNavigation { get; set; }
        public virtual Shop ShopIdFkNavigation { get; set; }
    }
}
