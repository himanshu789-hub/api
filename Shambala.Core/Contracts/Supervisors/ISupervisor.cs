using Shambala.Core.DTOModels;
using Shambala.Domain;

namespace Shambala.Core.Contracts.Supervisors
{
    public interface ISalesmanSupervisor : IGenericSupervisor<Salesman, SalesmanDTO>
    {

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