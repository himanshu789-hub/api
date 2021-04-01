using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;

namespace Shambala.Repositories
{
    public class ShopRepository:IShopRepository
    {
        ShambalaContext _context;
        public ShopRepository(ShambalaContext context)
        {
            _context = context;
        }

        public Shop Add(Shop entity)
        {
        }

        public bool Delete(int Id)
        {
            throw new System.NotImplementedException();
        }

        public Shop Get(int Id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Shop entity)
        {
            throw new System.NotImplementedException();
        }
    }
}