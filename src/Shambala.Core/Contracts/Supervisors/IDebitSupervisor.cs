using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    using Domain;
    using Models.DTOModel;
    public interface IDebitSupervisor
    {
        DebitDTO Add(DebitDTO credit);
        decimal GetLeftOverCredit(int outgoingShipmentId, short shopId);
        bool IsCreditCleared(int outgoingShipmentId, short shopId);
        IEnumerable<DebitDTO> GetLog(int outgoingShipmentId, int shopId);
    }
}