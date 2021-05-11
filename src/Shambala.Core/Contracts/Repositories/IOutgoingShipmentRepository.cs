using Shambala.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using Shambala.Core.Models.DTOModel;
namespace Shambala.Core.Contracts.Repositories
{

    using Models.BLLModel;
    public interface IOutgoingShipmentRepository : ILoadingProperties<OutgoingShipment>
    {
        OutgoingShipment Add(OutgoingShipment outgoingShipment);
        bool Return(int outgoingShipmentId, IEnumerable<OutgoingShipmentDetail> outgoingShipment);
        OutgoingShipment GetByIdWithNoTracking(int Id);
        IEnumerable<OutgoingShipment> GetShipmentsBySalesmnaIdAndDate(short salesmanId,System.DateTime date);
        IEnumerable<OutgoingShipmentDettailInfo> GetProductsById(int orderId);
        bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedStatus);
        bool Complete(int Id);
    }
}