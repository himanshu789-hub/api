using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    using Domain;
    using Models.DTOModel;
    public interface ICreditSupervisor
    {
        CreditDTO Add(CreditDTO credit);
        decimal GetLeftOverCredit(int outgoingShipmentId,short shopId);
        bool IsCrreditClared(int outgoingShipmentId,short shopId);
        IEnumerable<CreditDTO> GetLog(int outgoingShipmentId,int shopId);
    } 

}