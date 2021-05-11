using Shambala.Core.Models.DTOModel;
using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface ISalesmanSupervisor : IGenericSupervisor<SalesmanDTO>
    {
        IEnumerable<SalesmanDTO> GetAllActive();
    }
    public interface IInvoiceSupervisor : IGenericSupervisor<PostInvoiceDTO>
    {
    }
    public interface ISchemeSupervisor : IGenericSupervisor<SchemeDTO>
    {

    }
    public interface IShopSupervisor : IGenericSupervisor<ShopDTO>
    {
        
        IEnumerable<ShopDTO> GetAllByName(string name);
        ShopWithInvoicesDTO GetDetailWithInvoices(int Id);
    }
    public interface IIncomingShipmentSupervisor : IGenericSupervisor<ShipmentDTO>
    {

    }
}