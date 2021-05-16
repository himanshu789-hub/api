using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class PostInvoiceDTO
    {
        [Required]
        public short ShopId { get; set; }
        
        [Required]
        public int OutgoingShipmentId { get; set; }
        
        [Required]
        public byte CaretSize { get; set; }
        
        [Required]
        public System.DateTime DateCreated { get; set; }
        public short? SchemeId { get; set; }
        
        [Required]
        public IEnumerable<SoldItemsDTO> Invoices{get;set;}
        
    }
    public class SoldItemsDTO
    {
        
        [Required]
        public short Quantity { get; set; }
       
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public byte FlavourId { get; set; }
    }
    public class InvoiceDTO
    {
        [Required]
        public short ShopId { get; set; }
        
        [Required]
        public int OutgoingShipmentId { get; set; }
        
        [Required]
        public byte CaretSize { get; set; }
        
        [Required]
        public System.DateTime DateCreated { get; set; }
        
        public short? SchemeId { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public int FlavourId { get; set; }
    }
}