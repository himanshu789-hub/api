using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;

namespace Shambala.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        ShambalaContext _context;
        public InvoiceRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }
    }
}