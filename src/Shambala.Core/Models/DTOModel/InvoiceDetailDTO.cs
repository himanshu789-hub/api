using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;
    public class InvoiceDetailDTO
    {
        [RequiredWithNonDefault]
        public int Id { get; set; }
        
        [RequiredWithNonDefault]
        public System.DateTime DateCreated { get; set; }
        
        [RequiredWithNonDefault]
        public decimal TotalCostPrice { get; set; }
        
        public SchemeDTO Scheme { get; set; }
        
        [RequiredWithNonDefault]
        public decimal TotalSellingPrice { get; set; }
        
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        
        [RequiredWithNonDefault]
        public short ShopId { get; set; }
        
        public decimal TotalDuePrice { get; set; }
        public bool IsCompleted { get { return Utility.IsDueCompleted(this.TotalDuePrice); } }
    }
    public class InvoicewithCreditLogDTO : InvoiceDetailDTO
    {
        public OutgoingShipmentWithSalesmanInfoDTO OutgoingShipment { get; set; }
        public ShopDTO Shop { get; set; }
        public IEnumerable<CreditDTO> Credits { get; set; }

    }
    public class InvoiceBillDTO : InvoiceDetailDTO
    {
        public OutgoingShipmentWithSalesmanInfoDTO OutgoingShipment { get; set; }
        public ShopDTO Shop { get; set; }
        public IEnumerable<Models.BLLModel.InvoiceBillingInfoBLL> BillingInfo { get; set; }
    }
}