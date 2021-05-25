using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Repositories
{
    using Models.DTOModel;
    using Models.BLLModel;
    using Helphers;
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        decimal GetAggreate(int outgoingShipmentId,int shopId);
        IEnumerable<InvoiceDetailBLL> GetInvoicesByShopId(int shopId,System.DateTime? date,InvoiceStatus? status,int page,int count);
        InvoiceDetailWithInfoBLL GetInvoiceByShopAndShipmentId(short shopId,int shipmentId);
    }
    public interface ISalesmanRepository : IGenericRepository<Salesman>,IQueryList<Salesman>
    {
        IEnumerable<Salesman> GetAllActive();

    }
    public interface ISchemeRepository : IGenericRepository<Scheme>,IQueryList<Scheme>
    {

        Scheme GetSchemeWithNoTrackingById(short schemeId);
        IEnumerable<Scheme> GetAll();
    }
    public interface IIncomingShipmentRepository : IGenericRepository<IncomingShipment>
    {

    }
    public interface IShopRepository : IGenericRepository<Shop>,IQueryList<Shop>
    {
        Shop GetWithInvoiceDetail(int Id);
        IEnumerable<Shop> GetAllByName(string name);
    }
}