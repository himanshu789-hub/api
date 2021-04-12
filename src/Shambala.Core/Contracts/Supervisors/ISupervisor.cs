using Shambala.Core.DTOModels;
using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface ISalesmanSupervisor : IGenericSupervisor<Salesman, SalesmanDTO>
    {
           IEnumerable<SalesmanDTO> GetAllActive(); 
    }
    public interface IInvoiceSupervisor : IGenericSupervisor<Invoice, InvoiceDTO>
    {

    }
    public interface ISchemeSupervisor : IGenericSupervisor<Scheme, SchemeDTO>
    {

    }
    public interface IShopSupervisor : IGenericSupervisor<Shop, ShopDTO>
    {
    ShopWithInvoicesDTO GetDetailWithInvoices(int Id);
    }
    public interface IIncomingShipmentSupervisor : IGenericSupervisor<IncomingShipment,IncomingShipmentDTO>
    { 
        
    }
}