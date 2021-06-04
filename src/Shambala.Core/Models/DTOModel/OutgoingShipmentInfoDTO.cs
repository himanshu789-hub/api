using System.Collections.Generic;
namespace Shambala.Core.Models.DTOModel
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte CaretSize { get; set; }
    }
    public class OutgoingShipmentProductInfoDTO
    {
        public ProductInfo Product { get; set; }
        public FlavourDTO Flavour { get; set; }
    }
}