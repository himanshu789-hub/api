using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Scheme
    {
        public Scheme()
        {
            Invoice = new HashSet<Invoice>();
            Shop = new HashSet<Shop>();
        }

        public short Id { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsUserDefinedScheme { get; set; }
        public byte SchemeType { get; set; }
        public decimal Value { get; set; }

        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<Shop> Shop { get; set; }
    }
}
