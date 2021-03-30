using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class Salesman
    {
        public Salesman()
        {
            OutgoingShipment = new HashSet<OutgoingShipment>();
        }

        public short Id { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<OutgoingShipment> OutgoingShipment { get; set; }
    }
}
