
namespace Shambala.Core.Models
{
    public class ProductOutOfStockBLL : System.Collections.Generic.EqualityComparer<ProductOutOfStockBLL>
    {
        public int ProductId{get;set;}
        public int FlavourId{get;set;}
        public int Quantity{get;set;}

        public override bool Equals(ProductOutOfStockBLL x, ProductOutOfStockBLL y)
        {
           return x.FlavourId==y.FlavourId && x.ProductId == y.ProductId;
        }

        public override int GetHashCode(ProductOutOfStockBLL obj)
        {
            throw new System.NotImplementedException();
        }
    }
}