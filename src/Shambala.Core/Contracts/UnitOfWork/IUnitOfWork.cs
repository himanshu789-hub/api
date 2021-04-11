using Shambala.Core.Contracts.Repositories;
using System.Threading.Tasks;
using System.Data;
using System;
namespace Shambala.Core.Contracts.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IShopRepository ShopRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IOutgoingShipmentRepository OutgoingShipmentRepository { get; }
        IProductRepository ProductRepository { get; }
        ISalesmanRepository SalesmanRepository { get; }
        IIncomingShipmentRepository IncomingShipmentRepository{get;}
        ISchemeRepository SchemeRepository { get; }
        void SaveChanges();
        void BeginTransaction(IsolationLevel levl);
        Task<int> SaveChangesAsync();
        void Rollback();
    }
}