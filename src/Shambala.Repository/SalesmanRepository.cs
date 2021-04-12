using System.Collections.Generic;
using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Shambala.Repository
{
    public class SalesmanRepository : GenericRepository<Salesman>, ISalesmanRepository
    {
        ShambalaContext _context;
        public SalesmanRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Salesman> GetAllActive()
        {
            return _context.Salesman.Where(e => e.IsActive).ToList();
        }
    }
}