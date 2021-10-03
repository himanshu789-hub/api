
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

        public void Update(Ledger ledger)
        {
            context.Ledger.Update(ledger);
        }
    }
}