using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    using Helphers;

    public class PostInvoiceDTO
    {
        [RequiredWithNonDefault]
        public short ShopId { get; set; }
        
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        
        [RequiredWithNonDefault]
        public byte CaretSize { get; set; }
        
        [RequiredWithNonDefault]
        public System.DateTime DateCreated { get; set; }
        public short? SchemeId { get; set; }
        
        [Required]
        public IEnumerable<SoldItemsDTO> Invoices{get;set;}
        
    }
    public class SoldItemsDTO
    {
        
        [RequiredWithNonDefault]
        public short Quantity { get; set; }
       
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        
        [RequiredWithNonDefault]
        public byte FlavourId { get; set; }
    }
    public class InvoiceDTO
    {
        [RequiredWithNonDefault]
        public short ShopId { get; set; }
        
        [RequiredWithNonDefault]
        public int OutgoingShipmentId { get; set; }
        
        [RequiredWithNonDefault]
        public byte CaretSize { get; set; }
        
        [RequiredWithNonDefault]
        public System.DateTime DateCreated { get; set; }
        
        public short? SchemeId { get; set; }
        
        [RequiredWithNonDefault]
        public int Quantity { get; set; }
        
        [RequiredWithNonDefault]
        public int ProductId { get; set; }
        
        [RequiredWithNonDefault]
        public int FlavourId { get; set; }
    }
}