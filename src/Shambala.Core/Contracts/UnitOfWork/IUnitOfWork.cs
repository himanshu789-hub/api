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
        IDebitRepository DebitRepository{get;}
        ISalesmanRepository SalesmanRepository { get; }
        IIncomingShipmentRepository IncomingShipmentRepository{get;}
        ISchemeRepository SchemeRepository { get; }
        int SaveChanges();
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction BeginTransaction(IsolationLevel levl);
        Task<int> SaveChangesAsync();
        void Rollback();
    }
}