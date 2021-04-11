using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class CaretDetail
    {
        public byte Id { get; set; }
        public byte CaretSize { get; set; }
        public byte Gstrate { get; set; }
        public decimal CaretPrice { get; set; }
        public int? ProductIdFk { get; set; }

        public virtual Product ProductIdFkNavigation { get; set; }
    }
}
