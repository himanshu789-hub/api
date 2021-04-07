using Shambala.Domain;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {

    }
    public interface ISalesmanRepository : IGenericRepository<Salesman>
    {
      
    }
    public interface ISchemeRepository : IGenericRepository<Scheme>
    {

    }
    public interface IShopRepository : IGenericRepository<Shop>
    {
       Shop GetWithInvoiceDetail(int Id);
    }
}