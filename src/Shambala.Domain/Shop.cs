using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Shop
    {
        public Shop()
        {
            Invoice = new HashSet<Invoice>();
        }

        public string Title { get; set; }
        public string Address { get; set; }
        public short SchemeIdFk { get; set; }
        public short Id { get; set; }

        public virtual Scheme SchemeIdFkNavigation { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
