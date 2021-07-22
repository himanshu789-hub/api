
namespace Shambala.Core.Models.BLLModel
{
    public class ProductOutOfStockBLL : System.Collections.Generic.EqualityComparer<ProductOutOfStockBLL>
    {
        public int ProductId{get;set;}
        public int FlavourId{get;set;}
        public int Quantity{get;set;}
    }
}