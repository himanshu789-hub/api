using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Models.BLLModel
{

    public class InvoiceAggreagateBLL
    {
        public System.DateTime DateCreated { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalSellingPrice { get; set; }
        public decimal TotalDuePrice { get; set; }
        public int OutgoingShipmentId { get; set; }
        public short ShopId { get; set; }

    }
    public class InvoiceAggreagateDetailBLL
    {
        public System.DateTime DateCreated { get; set; }
        public decimal TotalCostPrice { get; set; }
        public Scheme Scheme { get; set; }
        public decimal TotalSellingPrice { get; set; }
        public decimal TotalDuePrice { get; set; }
        public int OutgoingShipmentId { get; set; }
        public short ShopId { get; set; }
    }
    public class InvoiceDetailWithInfoBLL : InvoiceAggreagateDetailBLL
    {
        public OutgoingShipment OutgoingShipment { get; set; }
        public Shop Shop { get; set; }
    }
    public class InvoicewithCreditLogBLL : InvoiceDetailWithInfoBLL
    {
        public IEnumerable<Credit> Credits { get; set; }
    }
    public class InvoiceBillingInfoBLL
    {
        public string ProductName { get; set; }
        public string FlavourName { get; set; }
        public byte CaretSize { get; set; }
        public short QuantityPurchase { get; set; }
        public System.DateTime DateCreated { get; set; }
        public byte QuantityDefected { get; set; }
        public byte Gstrate { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
    public class ShopBillInfo : InvoiceDetailWithInfoBLL
    {
          public IEnumerable<InvoiceBillingInfoBLL> BillingInfoBLLs{get;set;}
    }
}