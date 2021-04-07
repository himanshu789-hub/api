using System;
using System.Collections.Generic;

namespace Shambala.Core.DTOModels
{
    public class OutgoingShipment
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public virtual SalesmanDTO Salesman { get; set; }
        public virtual ICollection<InvoiceDTO> Invoice { get; set; }
        public virtual ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }

    }
    public class OutgoingShipmentDetailDTO
    {

        public int Id { get; set; }
        public byte CaretSize { get; set; }
        public short TotalQuantityShiped { get; set; }
        public byte TotalQuantityRejected { get; set; }
        public int OutgoingShipmentIdFk { get; set; }
        public  FlavourDTO Flavour { get; set; }
        public virtual ProductDTO Product { get; set; }
    }
}