namespace Shambala.Repository
{
    using System;
    using Core.Contracts.Repositories;
    using Shambala.Domain;
    using Shambala.Infrastructure;

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

        public decimal GetCreditAgggreate(int outgoingShipmentId, short shopId)
        {
            throw new NotImplementedException();
        }
    }
}