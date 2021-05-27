using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;

namespace Shambala.Core.Contracts.Supervisors
{
    using Helphers;
    public interface IFetchSupervisor<TDTO> where TDTO : class
    {
        bool IsNameAlreadyExists(string name, int? Id);
        IEnumerable<TDTO> GetAllByName(string name);
    }
    public interface ISalesmanSupervisor : IGenericSupervisor<SalesmanDTO>, IFetchSupervisor<SalesmanDTO>
    {
        IEnumerable<SalesmanDTO> GetAllActive();
    }
    
    public interface ISchemeSupervisor : IGenericSupervisor<SchemeDTO>, IFetchSupervisor<SchemeDTO>
    {
        IEnumerable<SchemeDTO> GetAll();
        SchemeDTO GetByShopId(int shopId);

    }
    public interface IShopSupervisor : IGenericSupervisor<ShopDTO>, IFetchSupervisor<ShopDTO>
    {
        ShopWithInvoicesDTO GetDetailWithInvoices(int Id);
    }
    public interface IIncomingShipmentSupervisor : IGenericSupervisor<ShipmentDTO>
    {

    }
}