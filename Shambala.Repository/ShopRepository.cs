using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Shambala.Repository
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        ShambalaContext _context;
        public ShopRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public Shop GetWithInvoiceDetail(int Id)
        {
            return _context.Shop.Include(e => e.Invoice).FirstOrDefault(e => e.Id == Id);
        }
    }
}