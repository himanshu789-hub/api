using Shambala.Domain;
using System.Collections.Generic;
using Shambala.Core.Models.DTOModel;
using System;
namespace Shambala.Core.Contracts.Repositories
{
    using Models.BLLModel;
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllWithNoTracking(DateTime? beforeDate=null);
        bool AddQuantity(int productId,int flavourId,int quantity);
        ProductInfoDTO GetProductsInStockWithDispatchQuantity(int ProductId,byte? FlavourId); 
        bool ReturnQuantity(IEnumerable<ProductReturnBLL> productReturnBLLs);
        bool DeductQuantityOfProductFlavour(int productId,int flavourId,int quantity);
   }
} 