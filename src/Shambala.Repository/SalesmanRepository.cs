using System.Collections.Generic;
using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq;
using System;

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


        public bool IsNameAlreadyExists(string name, short? Id)
        {
            bool IsNameExists = false;
            if (Id.HasValue)
                IsNameExists = _context.Salesman.FirstOrDefault(e => e.Id != Id && e.FullName.Equals(name)) != null;
            IsNameExists = _context.Salesman.FirstOrDefault(e => e.FullName.Equals(name)) != null;
            return IsNameExists;
        }
    }

}