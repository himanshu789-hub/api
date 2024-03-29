using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Shambala.Repository
{
    public class SchemeRepository : GenericRepository<Scheme>, ISchemeRepository
    {
        ShambalaContext _context;
        public SchemeRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Scheme> GetAll()
        {
            return _context.Scheme.ToList();
        }


        public Scheme GetSchemeWithNoTrackingById(short schemeId) =>
            _context.Scheme.AsNoTracking().First(e => e.Id == schemeId);

    }
}