using System.Collections.Generic;


namespace Shambala.Core.DTOModels
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public byte CaretSize { get; set; }
        public byte GSTRate { get; set; }
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