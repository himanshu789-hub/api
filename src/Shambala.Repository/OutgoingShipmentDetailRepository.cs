using System.Linq;
namespace Shambala.Repository
{
    using Shambala.Infrastructure;
    using Shambala.Core.Contracts.Repositories;
    using Shambala.Domain;

    public class OutgoingShipmentDetailRepository : GenericRepository<OutgoingShipmentDetails>, IOutgoingShipmentDetailRepository
    {
        readonly ShambalaContext context;
        public OutgoingShipmentDetailRepository(ShambalaContext context) : base(context)
        {
            this.context = context;
        }

        public bool Delete(long Id)
        {
            context.OutgoingShipmentDetails.Remove(new OutgoingShipmentDetails() { Id = Id });

            return context.SaveChanges() > 0;
        }
    }
}