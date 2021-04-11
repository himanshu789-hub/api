using System;
using System.Collections.Generic;

namespace Shambala.Domain
{
    public partial class IncomingShipment
    {
        public int Id { get; set; }
        public int ProductIdFk { get; set; }
        public short TotalRecievedPieces { get; set; }
        public short TotalDefectPieces { get; set; }
        public byte CaretSize { get; set; }
        public DateTime DateCreated{get;set;}
        public virtual Product ProductIdFkNavigation { get; set; }

    }
}
