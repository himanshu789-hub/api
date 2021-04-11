using System;
using System.Collections.Generic;

namespace Shambala.Core.DTOModels
{
    public class OutgoingShipmentDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public virtual SalesmanDTO Salesman { get; set; }
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
        public ProductDTO Product { get; set; }
    }
}