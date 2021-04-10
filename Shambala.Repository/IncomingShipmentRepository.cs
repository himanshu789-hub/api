using Shambala.Core.Contracts.Repositories;
using Shambala.Domain;
using Shambala.Infrastructure;
namespace Shambala.Repository
{
    public class IncomingShipmentRepository : GenericRepository<IncomingShipment>, IIncomingShipmentRepository
    {
        public IncomingShipmentRepository(ShambalaContext context) : base(context)
        {

        }
    }
}