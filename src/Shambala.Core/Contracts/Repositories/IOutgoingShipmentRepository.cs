using Shambala.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IOutgoingShipmentRepository : ILoadingProperties<OutgoingShipment>
    {
        OutgoingShipment Add(OutgoingShipment outgoingShipment);
        bool Return(int outgoingShipmentId ,IEnumerable<OutgoingShipmentDetail> outgoingShipment);
        OutgoingShipment GetByIdWithNoTracking(int Id);
        IEnumerable<OutgoingShipmentDettailInfo> GetProductsById(int orderId);
        bool CheckStatus(int Id, OutgoingShipmentStatus status);
    }
}