namespace Shambala.Core.DTOModels
{
    public class ProductInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte CaretSize{get;set;}
    }
    public class OutgoingShipmentDettailInfo
    {
        public  ProductInfo Product { get; set; }
        public  FlavourDTO Flavour { get; set; }
    }
}