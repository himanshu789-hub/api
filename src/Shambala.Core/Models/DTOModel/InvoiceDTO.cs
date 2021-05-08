using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int OutgoingShipmentId { get; set; }
        public byte CaretSize { get; set; }
        public System.DateTime DateCreated { get; set; }
        
        public short? SchemeId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int FlavourId { get; set; }
    }
}