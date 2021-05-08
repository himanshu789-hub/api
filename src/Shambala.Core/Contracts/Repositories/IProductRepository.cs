using Shambala.Domain;
using System.Collections.Generic;
using Shambala.Core.Models.DTOModel;
namespace Shambala.Core.Contracts.Repositories
{
    using Models.BLLModel;
    public interface IProductRepository
    {
        
        IEnumerable<Product> GetAllWithNoTracking();
        bool AddQuantity(int productId,int flavourId,int quantity);
        IEnumerable<ProductInfoDTO> GetProductsInStockWithDispatchQuantity(); 
        bool ReturnQuantity(IEnumerable<ProductReturnBLL> productReturnBLLs);
        bool DeductQuantityOfProductFlavour(int productId,int flavourId,int quantity);
   }
} 