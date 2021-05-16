using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Shambala.Repository
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        ShambalaContext _context;
        public ShopRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Shop> GetAllByName(string name)
        {
            return _context.Shop.Include(e=>e.SchemeIdFkNavigation)
            .Where(e => e.Title.Contains(name))
            .AsNoTracking()
            .ToList();
        }

        public Shop GetWithInvoiceDetail(int Id)
        {
            return _context.Shop.Include(e => e.Invoice).FirstOrDefault(e => e.Id == Id);
        }
    }
}