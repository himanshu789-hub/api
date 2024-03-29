using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;

namespace Shambala.Core.Contracts.Supervisors
{
    using Helphers;
    public interface IFetchSupervisor<TDTO> where TDTO : class
    {
        //bool IsNameAlreadyExists(string name, object Id);
        IEnumerable<TDTO> GetAllByName(string name);
    }
    public interface ISalesmanSupervisor : IGenericSupervisor<SalesmanDTO>, IFetchSupervisor<SalesmanDTO>
    {
        bool IsNameAlreadyExists(string name, short? Id);
        IEnumerable<SalesmanDTO> GetAllActive();
    }

    public interface ISchemeSupervisor : IGenericSupervisor<SchemeDTO>
    {
        IEnumerable<SchemeDTO> GetAll();

    }
    public interface IShopSupervisor : IGenericSupervisor<ShopDTO>, IFetchSupervisor<ShopDTO>
    {
        //    ShopWithInvoicesDTO GetDetailWithInvoices(int Id);
        bool IsNameAlreadyExists(string name, int? Id);
    }
    public interface IIncomingShipmentSupervisor : IGenericSupervisor<ShipmentDTO>
    {

    }
}