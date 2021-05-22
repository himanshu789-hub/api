
using System;
using System.Linq;
using Shambala.Domain;
using Shambala.Infrastructure;
namespace Shambala.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Contracts.Repositories;

    public class CreditRepository : ICreditRepository
    {
        ShambalaContext context;
        public CreditRepository(ShambalaContext context) => this.context = context;
        public Credit Add(int outgoingShipmentId, short shopId, decimal amount, DateTime date)
        {
            Credit credit = new Credit { Amount = amount, OutgoingShipmentIdFk = outgoingShipmentId, ShopIdFk = shopId, DateRecieved = date.ToUniversalTime() };
            context.Credit.Add(credit);
            return credit;
        }

        public IEnumerable<Credit> FetchList(Func<Credit, bool> predicate)
        {
            return context.Credit.Where(predicate).ToList();
        }

        public decimal GetCreditAgggreate(int outgoingShipmentId, short shopId)
        {
            return context.Credit.Where(e => e.OutgoingShipmentIdFk == outgoingShipmentId && e.ShopIdFk == shopId).Sum(e => e.Amount);
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