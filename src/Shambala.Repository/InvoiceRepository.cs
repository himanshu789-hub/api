using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq;
namespace Shambala.Repository
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        ShambalaContext _context;
        public InvoiceRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public decimal GetAggreate(int outgoingShipmentId, int shopId)
        {
            return _context.Invoice.Where(e=>e.OutgoingShipmentIdFk==outgoingShipmentId && e.SchemeIdFk == shopId).Sum(e=>e.SellingPrice);
        }
    }
}