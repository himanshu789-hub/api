using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Shop
    {
        public Shop()
        {
            Credit = new HashSet<Credit>();
            Debit = new HashSet<Debit>();
        }

        public string Title { get; set; }
        public string Address { get; set; }
        public short Id { get; set; }

        public virtual ICollection<Credit> Credit { get; set; }
        public virtual ICollection<Debit> Debit { get; set; }
    }
}
