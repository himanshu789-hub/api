using Shambala.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using Shambala.Core.Models.DTOModel;
namespace Shambala.Core.Contracts.Repositories
{

    using Models;
    public interface IOutgoingShipmentRepository : ILoadingProperties<OutgoingShipment>
    {
        OutgoingShipment Add(OutgoingShipment outgoingShipment);
        OutgoingShipment GetByIdWithNoTracking(long Id);
        bool CheckStatusWithNoTracking(long Id, OutgoingShipmentStatus expectedStatus);
        //bool Complete(int Id);
        //  OutgoingShipment GetAllDetailById(int Id);
        IEnumerable<OutgoingShipment> GetBySalesmanIdAndAfterDate(short salesmanId, System.DateTime date);
        bool Update(OutgoingShipment outgoingShipment);
    }
    public interface IOutgoingShipmentDetailRepository : IGenericRepository<OutgoingShipmentDetails>
    {
        bool Delete(long Id);
    }
}