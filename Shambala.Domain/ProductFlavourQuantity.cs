using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class ProductFlavourQuantity
    {
        public byte Id { get; set; }
        public short Quantity { get; set; }
        public byte FlavourIdFk { get; set; }
        public int ProductIdFk { get; set; }

        public virtual Flavour FlavourIdFkNavigation { get; set; }
        public virtual Product ProductIdFkNavigation { get; set; }
    }
}
