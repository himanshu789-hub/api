using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class OutgoingShipment
    {
        public OutgoingShipment()
        {
            Invoice = new HashSet<Invoice>();
            OutgoingShipmentDetails = new HashSet<OutgoingShipmentDetail>();
        }

        public int Id { get; set; }
        public short SalesmanIdFk { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }

        public virtual Salesman SalesmanIdFkNavigation { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual ICollection<OutgoingShipmentDetail> OutgoingShipmentDetails { get; set; }

    }
}
