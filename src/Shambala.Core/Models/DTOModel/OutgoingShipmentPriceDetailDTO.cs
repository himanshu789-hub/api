using System.Collections.Generic;

namespace Shambala.Core.Models.DTOModel
{
    public class OutgoingShipmentPriceDetailDTO
    {
        public int Id { get; set; }
        public SalesmanDTO Salesman { get; set; }
        public ICollection<OutgoingShipmentProductDetailDTO> ProductDetails { get; set; }
    }
    public class OutgoingShipmentProductDetailDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public ICollection<OutgoingShipmentFlavourDetailDTO> OutgoingShipmentFlavourDetails { get; set; }
    }
    public class OutgoingShipmentFlavourDetailDTO
    {

        public int FlavourId { get; set; }
        public string Name { get; set; }
        public ICollection<FlavourQuantityVariantDetailDTO> FlavourQuantityVariantDetails { get; set; }
        public FlavourSchemeDetailDTO SchemeDetail { get; set; }
    }
    public class FlavourQuantityVariantDetailDTO
    {
        public decimal PricePerCarat { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
    public class FlavourSchemeDetailDTO
    {
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerBottle { get; set; }
    }

}