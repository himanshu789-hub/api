using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;

namespace Shambala.Repository
{
    public class SchemeRepository : GenericRepository<Scheme>, ISchemeRepository
    {
        ShambalaContext _context;
        public SchemeRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }
    }
}