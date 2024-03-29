using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shambala.Core.Models.DTOModel
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public byte CaretSize { get; set; }
        public byte GSTRate { get; set; }
        public decimal PricePerCaret{get;set;}
        public byte SchemeQuantity{get;set;}
        public decimal PricePerBottle{get;set;}
               
        public ICollection<FlavourDTO> Flavours { get; set; }

        public ProductDTO()
        {
            this.Flavours = new List<FlavourDTO>();
        }
    }
    public class FlavourDTO
    {
        public byte Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
    }

}