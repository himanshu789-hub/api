using Shambala.Domain;
using System.Collections.Generic;
namespace Shambala.Core.Contracts.Repositories
{
    using Models.DTOModel;
    using Models;
    using Helphers;
    // public interface IInvoiceRepository : IGenericRepository<Invoice>
    // {
    //     void MakeCompleted(int Id);
    // }
    public interface ICustomPriceRepository
    {
        CustomCaratPrice Add(CustomCaratPrice customCaratPrice);
        bool Delete(int customCaratPrice);
    }
    public interface ISalesmanRepository : IGenericRepository<Salesman>, IQueryList<Salesman>
    {
        bool IsNameAlreadyExists(string name, short? Id);
        IEnumerable<Salesman> GetAllActive();
    }

    public interface ISchemeRepository : IGenericRepository<Scheme>
    {

        Scheme GetSchemeWithNoTrackingById(short schemeId);
        IEnumerable<Scheme> GetAll();
    }
    public interface IIncomingShipmentRepository : IGenericRepository<IncomingShipment>
    {

    }
    public interface IShopRepository : IGenericRepository<Shop>, IQueryList<Shop>
    {
        bool IsNameAlreadyExists(string name, int? Id);
        Shop GetWithInvoiceDetail(int Id);
        IEnumerable<Shop> GetAllByName(string name);
    }
}