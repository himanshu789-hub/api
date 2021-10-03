using System.Collections.Generic;
namespace Shambala.Core.Models
{
    using Domain;
    using Helphers;
    using Models.DTOModel;
    public class OutgoingShipmentAggregateBLL
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public Salesman Salesman { get; set; }
        public Ledger Ledger{get;set;}
        public ICollection<OutgoingDetailBLL> OutgoingShipmentDetails { get; set; }
    }
    public class OutgoingDetailBLL : OutgoingShipmentDetails
    {
        public byte CaretSize { get; set; }
        public decimal UnitPrice { get; set; }
    }
}