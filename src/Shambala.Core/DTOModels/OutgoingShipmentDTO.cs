using System;
using System.Collections.Generic;

namespace Shambala.Core.DTOModels
{
    public class OutgoingShipmentDTO
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int SalesmanId { get; set; }
        public ICollection<OutgoingShipmentDetailDTO> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingShipmentDetailDTO
    {

        public int Id { get; set; }
        public byte CaretSize { get; set; }
        public short TotalQuantityShiped { get; set; }
        public byte TotalQuantityRejected { get; set; }
        public int OutgoingShipmentId { get; set; }
        public int FlavourId { get; set; }
        public int ProductId { get; set; }
    }
}