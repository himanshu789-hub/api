using Shambala.Infrastructure;
using Shambala.Domain;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Shambala.Repository
{
    public class ProductRepository : IProductRepository
    {
        ShambalaContext _context;
        public ProductRepository(ShambalaContext context) => _context = context;

        public bool AddQuantity(int productId, int flavourId, short quantity)
        {
            _context.Database.ExecuteSqlRaw("UPDATE ProductFlavourQuantity SET Quantity = Quantity + @Quantity WHERE Product_Id_FK = {0} AND Flavour_ID_FK = {1}", productId, flavourId);
            return true;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Product.Include(e => e.CaretDetail).Include(e => e.ProductFlavourQuantity).ToList();
        }
    }
}