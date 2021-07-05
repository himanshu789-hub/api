using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
namespace Shambala.Core.Contracts.Repositories
{
    using Domain;

    public interface IDebitRepository
    {
        Debit Add(int outgoingShipmentId, short shopId, decimal amount, System.DateTime date);
        decimal GetDebitAgggreate(int outgoingShipmentId, short shopId);
        
        IEnumerable<Debit> FetchList(Expression<Func<Debit,bool>>  expression);
        int SaveChanges();
        Task<int> SaveChangesAync();

    }
}