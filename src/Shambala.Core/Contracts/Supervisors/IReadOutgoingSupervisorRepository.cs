using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    using Models.DTOModel;
    using Domain;
    public interface IReadOutgoingSupervisor
    {
        IEnumerable<OutgoingShipmentInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, System.DateTime afterDate);
        OutgoingShipmentAggregateDTO GetAggregate(int Id);
    }
}