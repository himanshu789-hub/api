using System;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    public class InvoiceDetailDTO
    {
        public int Id{get;set;}
        public System.DateTime DateCreated{get;set;}
        public decimal TotalCostPrice{get;set;}
        public SchemeDTO Scheme{get;set;}
        public decimal TotalSellingPrice{get;set;}
        public int OutgoingShipmentId{get;set;}
        public short ShopId{get;set;}
        public decimal TotalDuePrice{get;set;}
    }
    public class InvoicewithCreditLogDTO:InvoiceDetailDTO
    {
          public OutgoingShipmentWithSalesmanInfoDTO OutgoingShipment{get;set;}
          public  ShopDTO Shop{get;set;}
               
    }
    
}