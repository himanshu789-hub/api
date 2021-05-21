
namespace Shambala.Core.Contracts.Repositories
{
    using Domain;
    
    public interface ICreditRepository
    {
          Credit Add(int outgoingShipmentId,short shopId,decimal amount,System.DateTime date);
          decimal GetCreditAgggreate(int outgoingShipmentId,short shopId);
    }
}