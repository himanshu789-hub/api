using Shambala.Core.Contracts.Repositories;
namespace Shambala.Core.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        IShopRepository ShopRepository { get; set; }
        IInvoiceRepository InvoiceRepository{get;set;}
        IOutgoingShipmentRepository OutgoingShipmentRepository{get;set;}
        IProductRepository ProductRepository{get;set;}
        ISalesmanRepository SalesmanRepository{get;set;}
        ISchemeRepository SchemeRepository{get;set;}        
        void SaveChanges();
        void BeginTransaction();
        void Commit();
    }
}