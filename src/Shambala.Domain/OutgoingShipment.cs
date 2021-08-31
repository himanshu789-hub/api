using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Shambala.Domain
{
    public partial class OutgoingShipment
    {
        public OutgoingShipment()
        {
            Debit = new HashSet<Debit>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetails>();
        }

        public int Id { get; set; }
        public short SalesmanIdFk { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }

        public virtual Salesman SalesmanIdFkNavigation { get; set; }
        public virtual ICollection<Debit> Debit { get; set; }
        public virtual ICollection<OutgoingShipmentDetails> OutgoingShipmentDetails { get; set; }
    }
}
