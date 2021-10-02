using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class Salesman
    {
        public Salesman()
        {
            OutgoingShipment = new HashSet<OutgoingShipment>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<OutgoingShipment> OutgoingShipment { get; set; }
    }
}
