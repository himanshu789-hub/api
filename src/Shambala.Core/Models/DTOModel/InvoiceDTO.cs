using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class PostInvoiceDTO
    {
        public short ShopId { get; set; }
        public int OutgoingShipmentId { get; set; }
        public byte CaretSize { get; set; }
        public System.DateTime DateCreated { get; set; }
        public short? SchemeId { get; set; }
        public IEnumerable<SoldItemsDTO> SoldItems{get;set;}
        
    }
    public class SoldItemsDTO
    {
        public short Quantity { get; set; }
        public int ProductId { get; set; }
        public int FlavourId { get; set; }
    }
    public class InvoiceDTO
    {
        
        public short ShopId { get; set; }
        public int OutgoingShipmentId { get; set; }
        public byte CaretSize { get; set; }
        public System.DateTime DateCreated { get; set; }
        public short? SchemeId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int FlavourId { get; set; }
    }
}