using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{

    using Helphers;
    using Models;
    public class OutgoingShipmentAggregateDTO
    {
        public int Id { get; set; }
        public SalesmanDTO Salesman { get; set; }
        public OutgoingShipmentStatus Status { get; set; }
        public IEnumerable<OutgoingShipmentAggegateDetailDTO> OutgoingShipmentDetails { get; set; }
        public decimal TotalSchemePrice { get; set; }
        public decimal TotalShipedPrice { get; set; }
        public decimal TotalNetPrice { get; set; }
        public short TotalSchemeQuantity{get;set;}
        public decimal CustomCaratTotalPrice { get; set; }

    }
    public class OutgoingShipmentAggegateDetailDTO : OutgoingShipmentDetailTransferDTO
    {
        public byte CaretSize { get; set; }
        public decimal UnitPrice { get; set; }
    }
}