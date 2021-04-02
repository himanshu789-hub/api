using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;

namespace Shambala.Repository
{
    public class SalesmanRepository : GenericRepository<Salesman>, ISalesmanRepository
    {
        ShambalaContext _context;
        public SalesmanRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }
    }
}