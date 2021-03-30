using Shambala.Core.Contracts.Repositories;

namespace Shambala.Core.Repositories
{
    public class ShopRepository:IShopRepository
    {
        ShambalaContext _context;
        public ShopRepository(ShambalaContext context)
        {
            _context = context;
        }
    }
}