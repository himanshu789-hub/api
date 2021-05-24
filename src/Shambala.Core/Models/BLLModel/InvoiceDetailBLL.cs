
using Shambala.Domain;
namespace Shambala.Core.Models.BLLModel
{
    public class InvoiceDetailBLL
    {
        public System.DateTime DateCreated{get;set;}
        public decimal CostPrice{get;set;}
        public Scheme Scheme{get;set;}
        public decimal SellingPrice{get;set;}
        public decimal DuePrice{get;set;}
        public int OutgoingShipmentId{get;set;}
        public short ShopId{get;set;}
    }
}