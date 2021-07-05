
using System;
using System.Linq;
using Shambala.Domain;
using Shambala.Infrastructure;
using System.Linq.Expressions;

namespace Shambala.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Contracts.Repositories;

    public class DebitRepository : IDebitRepository
    {
        ShambalaContext context;
        public DebitRepository(ShambalaContext context) => this.context = context;
        public Debit Add(int outgoingShipmentId, short shopId, decimal amount, DateTime date)
        {
            Debit credit = new Debit { Amount = amount, OutgoingShipmentIdFk = outgoingShipmentId, ShopIdFk = shopId, DateRecieved = date.ToUniversalTime() };
            context.Debit.Add(credit);
            return credit;
        }

        public IEnumerable<Debit> FetchList(Expression<Func<Debit, bool>> expression)
        {
            return context.Debit.Where(expression).ToList();
        }

        public decimal GetDebitAgggreate(int outgoingShipmentId, short shopId)
        {
            return context.Debit.Where(e => e.OutgoingShipmentIdFk == outgoingShipmentId && e.ShopIdFk == shopId).Sum(e => e.Amount);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAync()
        {
            return context.SaveChangesAsync();
        }
    }
}