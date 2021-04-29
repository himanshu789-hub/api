using Shambala.Domain;
using System.Collections.Generic;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        bool AddQuantity(int productId,int flavourId,short quantity);
        IEnumerable<ProductInfoDTO> GetProductsInStockWithDispatchQuantity(int Id); 
   }
} 