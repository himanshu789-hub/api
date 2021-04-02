using Shambala.Infrastructure;
using Shambala.Domain;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Shambala.Repository
{
    public class ProductRepository : IProductRepository
    {
        ShambalaContext _context;
        public ProductRepository(ShambalaContext context) => _context = context;
        public IEnumerable<Product> GetAll()
        {
            return _context.Product.Include(e => e.CaretDetail).Include(e => e.ProductFlavourQuantity).ToList();
        }
    }
}