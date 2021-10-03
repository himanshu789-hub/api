namespace Shambala.Core.Contracts.Repositories
{
    using Shambala.Domain;
    public interface ILedgerRespository
    {
        Ledger Add(Ledger ledger);
        void Update(Ledger ledger);
    }
}