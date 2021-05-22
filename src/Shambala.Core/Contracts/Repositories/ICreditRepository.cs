using System.Threading.Tasks;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Repositories
{
    using Domain;

    public interface ICreditRepository
    {
        Credit Add(int outgoingShipmentId, short shopId, decimal amount, System.DateTime date);
        decimal GetCreditAgggreate(int outgoingShipmentId, short shopId);
        IEnumerable<Credit> FetchList(System.Func<Credit,bool> predicate);
        int SaveChanges();
        Task<int> SaveChangesAync();

    }
}