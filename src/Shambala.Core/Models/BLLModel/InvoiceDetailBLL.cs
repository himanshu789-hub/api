using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Models.BLLModel
{

    public class InvoiceDetailBLL
    {
        public System.DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public Scheme Scheme { get; set; }
        public decimal TotalSellingPrice { get; set; }
        public decimal TotalDuePrice { get; set; }
        public int OutgoingShipmentId { get; set; }
        public short ShopId { get; set; }
    }
    public class InvoiceAggreateBLL
    {
        public decimal SoldPrice { get; set; }

        public decimal CostPrice { get; set; }

        public int OutgoingShipmentId { get; set; }
        public decimal DuePrice { get; set; }
    }
    public class InvoiceDetailWithInfoBLL : InvoiceDetailBLL
    {
        public OutgoingShipment OutgoingShipment { get; set; }
        public Shop Shop { get; set; }

    }
    public class InvoicewithCreditLogBLL : InvoiceDetailWithInfoBLL
    {
        public IEnumerable<Credit> Credits { get; set; }
    }
}