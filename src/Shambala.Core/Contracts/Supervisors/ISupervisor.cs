using Shambala.Core.DTOModels;
using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface ISalesmanSupervisor : IGenericSupervisor<SalesmanDTO>
    {
           IEnumerable<SalesmanDTO> GetAllActive(); 
    }
    public interface IInvoiceSupervisor : IGenericSupervisor<InvoiceDTO>
    {

    }
    public interface ISchemeSupervisor : IGenericSupervisor< SchemeDTO>
    {

    }
    public interface IShopSupervisor : IGenericSupervisor<ShopDTO>
    {
    ShopWithInvoicesDTO GetDetailWithInvoices(int Id);
    }
    public interface IIncomingShipmentSupervisor : IGenericSupervisor<IncomingShipmentDTO>
    { 
        
    }
}