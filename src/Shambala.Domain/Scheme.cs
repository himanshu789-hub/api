using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Scheme
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public byte Quantity { get; set; }
        public int ProductIdFk { get; set; }

        public virtual Product ProductIdFkNavigation { get; set; }
    }
}
