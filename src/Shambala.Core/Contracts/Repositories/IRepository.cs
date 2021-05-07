using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {

    }
    public interface ISalesmanRepository : IGenericRepository<Salesman>
    {
         IEnumerable<Salesman> GetAllActive();  
    }
    public interface ISchemeRepository : IGenericRepository<Scheme>
    {

    }
    public interface IIncomingShipmentRepository:IGenericRepository<IncomingShipment>
    {
        
    }
    public interface IShopRepository : IGenericRepository<Shop>
    {
       Shop GetWithInvoiceDetail(int Id);
    }
}