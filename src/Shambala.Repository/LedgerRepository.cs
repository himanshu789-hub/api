using System.Linq;
namespace Shambala.Repository
{
    using Infrastructure;
    using Core.Contracts.Repositories;
    using Shambala.Domain;

    public class LedgerRepository : ILedgerRespository
    {
        ShambalaContext context;
        public LedgerRepository(ShambalaContext context)
        {

            this.context = context;
        }
        public Ledger Add(Ledger ledger)
        {
            ledger.Id = 0;
            var entity = context.Ledger.Add(ledger);
            return entity.Entity;
        }

        public bool DoLedgerExistsForShipment(int Id)
        {
            return context.Ledger.Count(e => e.OutgoingShipmentIdFk == Id) > 0;
        }

        public void Update(Ledger ledger)
        {
            context.Ledger.Update(ledger);
        }
    }
}