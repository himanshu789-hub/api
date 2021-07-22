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
        OutgoingShipment GetByIdWithNoTracking(int Id);
        bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedStatus);
        bool Complete(int Id);
        OutgoingShipment GetAllDetailById(int Id);
    }
    public interface IOutgoingShipmentDetailRepository : IGenericRepository<OutgoingShipmentDetails>
    {
       bool Delete(int Id);
    }
}