namespace Shambala.Core.Contracts.Repositories
{
    using Shambala.Domain;
    public interface ILedgerRespository
    {
        Ledger Add(Ledger ledger);
        
        Ledger FindByShipmentId(int OutgoingShipmentId);
        void Update(Ledger ledger);
    }
}