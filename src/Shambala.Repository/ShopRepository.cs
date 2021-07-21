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
            return _context.Shop.Where(e => e.Title.Contains(name))
            .AsNoTracking()
            .ToList();
        }

        public Shop GetWithInvoiceDetail(int Id)
        {
            //   return _context.Shop.Include(e => e.Invoice).FirstOrDefault(e => e.Id == Id);
            throw new System.NotImplementedException();
        }

        public bool IsNameAlreadyExists(string name, int? Id)
        {
            bool IsNameExists = false;
            if (Id.HasValue)
                IsNameExists = _context.Shop.FirstOrDefault(e => e.Id != Id && e.Title.Equals(name)) != null;
            IsNameExists = _context.Shop.FirstOrDefault(e => e.Title.Equals(name)) != null;
            return IsNameExists;
        }
    }
}