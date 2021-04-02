using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;

namespace Shambala.Repository
{
    public class ShopRepository : GenericRepository<Shop>, IShopRepository
    {
        ShambalaContext _context;
        public ShopRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }
    }
}