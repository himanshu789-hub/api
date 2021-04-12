using Shambala.Infrastructure;
using Shambala.Core.Contracts.UnitOfWork;
using Shambala.Core.Contracts.Repositories;
using Shambala.Repository;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
namespace Shambala.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        bool _isTransactionRollback = false;
        bool _isTransactionCommited = false;
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction;
        ShambalaContext _context;
        IShopRepository _shopRepository { get; set; }
        IInvoiceRepository _invoiceRepository { get; set; }
        IOutgoingShipmentRepository _outgoingShipmentRepository { get; set; }
        IProductRepository _productRepository { get; set; }
        ISalesmanRepository _salesRepository { get; set; }
        IIncomingShipmentRepository _incomingShipmentRepository { get; set; }
        ISchemeRepository _schemeRepository { get; set; }
        public IShopRepository ShopRepository
        {
            get
            {
                return _shopRepository = _shopRepository == null ? new ShopRepository(_context) : ShopRepository;
            }

        }
        public IIncomingShipmentRepository IncomingShipmentRepository
        {
            get
            {
                return _incomingShipmentRepository = _incomingShipmentRepository == null ? new IncomingShipmentRepository(context: _context) : _incomingShipmentRepository;
            }
        }
        public IInvoiceRepository InvoiceRepository
        {
            get
            {
                return _invoiceRepository = _invoiceRepository == null ? new InvoiceRepository(_context) : _invoiceRepository;
            }
        }

        public IOutgoingShipmentRepository OutgoingShipmentRepository
        {
            get
            {
                return _outgoingShipmentRepository = _outgoingShipmentRepository == null ? new OutgoingShipmentRepository(_context) : _outgoingShipmentRepository;
            }
        }
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository = _productRepository == null ? new ProductRepository(_context) : _productRepository;
            }
        }

        public ISalesmanRepository SalesmanRepository
        {
            get
            {
                return _salesRepository = _salesRepository == null ? new SalesmanRepository(_context) : _salesRepository;
            }
        }
        public ISchemeRepository SchemeRepository
        {
            get
            {
                return _schemeRepository = _schemeRepository == null ? new SchemeRepository(_context) : _schemeRepository;
            }
        }
        ILogger<UnitOfWork> logger;
        public UnitOfWork(ShambalaContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public int SaveChanges()
        {
            int value = -3345;
            if (transaction != null)
            {
                try
                {
                    value = _context.SaveChanges();
                    transaction.Commit();
                    _isTransactionCommited = true;
                }
                catch (System.Exception e)
                {
                    this.Rollback();
                    logger.LogCritical(e.ToString());
                }
            }
            else
            {
                value = _context.SaveChanges();
            }
            return value;
        }


        public void BeginTransaction(IsolationLevel levl = IsolationLevel.ReadCommitted)
        {
            transaction = _context.Database.BeginTransaction(levl);
        }


        public void Rollback()
        {
            if (transaction != null)
                transaction.Rollback();
            _isTransactionRollback = true;
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                if (!_isTransactionCommited && !_isTransactionRollback)
                    transaction.Rollback();
            }
            _context.Dispose();
            System.GC.Collect();
        }

        public async Task<int> SaveChangesAsync()
        {
            int value = -1;
            if (transaction != null)
            {
                try
                {

                    value = await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _isTransactionCommited = true;
                }
                catch (System.Exception e)
                {
                    this.Rollback();
                    logger.LogCritical(e.ToString());
                }
            }

            else
                value = await _context.SaveChangesAsync();

            return value;
        }
    }
}