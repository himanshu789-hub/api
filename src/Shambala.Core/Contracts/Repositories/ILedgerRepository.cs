namespace Shambala.Core.Contracts.Repositories
{
    using Shambala.Domain;
    public interface ILedgerRespository
    {
        Ledger Add(Ledger ledger);
        
        bool DoLedgerExistsForShipment(int Id);
        void Update(Ledger ledger);
    }
}