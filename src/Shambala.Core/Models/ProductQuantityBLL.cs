using System;
namespace Shambala.Core.Models
{
    public class ProductQuantityBLL:IEquatable<ProductQuantityBLL>
    {
        public int ProductId { get; set; }
        public short FlavourId { get; set; }
        public int Quantity { get; set; }

        public bool Equals(ProductQuantityBLL other)
        {
            return this.FlavourId==other.FlavourId && this.ProductId==other.ProductId;
        }
    }
}