using Shambala.Infrastructure;
using Shambala.Core.Contracts.UnitOfWork;
using Shambala.Core.Contracts.Repositories;
using Shambala.Repository;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
namespace Shambala.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDbContextTransaction CurrentTransaction { get { return transaction; } }
        bool _isTransactionRollback = false;
        bool _isTransactionCommited = false;
        IDbContextTransaction transaction;
        ShambalaContext _context;
        IShopRepository _shopRepository { get; set; }
        IDebitRepository _debitRepository { get; set; }
        // IInvoiceRepository _invoiceRepository { get; set; }
        IOutgoingShipmentRepository _outgoingShipmentRepository { get; set; }
        IProductRepository _productRepository { get; set; }
        ISalesmanRepository _salesRepository { get; set; }
        IIncomingShipmentRepository _incomingShipmentRepository { get; set; }
        ISchemeRepository _schemeRepository { get; set; }
        IOutgoingShipmentDetailRepository _outgoingShipmentDetailRepository { get; set; }
        public IShopRepository ShopRepository
        {
            get
            {
                return _shopRepository = _shopRepository == null ? new ShopRepository(_context) : _shopRepository;
            }

        }
        public IIncomingShipmentRepository IncomingShipmentRepository
        {
            get
            {
                return _incomingShipmentRepository = _incomingShipmentRepository == null ? new IncomingShipmentRepository(context: _context) : _incomingShipmentRepository;
            }
        }
        // public IInvoiceRepository InvoiceRepository
        // {
        //     get
        //     {
        //         return _invoiceRepository = _invoiceRepository == null ? new InvoiceRepository(_context) : _invoiceRepository;
        //     }
        // }

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

        public IDebitRepository DebitRepository
        {
            get
            {
                return _debitRepository = _debitRepository == null ? new DebitRepository(_context) : _debitRepository;
            }
        }
        public IOutgoingShipmentDetailRepository OutgoingShipmentDetailRepository
        {
            get
            {
                return _outgoingShipmentDetailRepository = _outgoingShipmentDetailRepository == null ? new OutgoingShipmentDetailRepository(_context) : _outgoingShipmentDetailRepository;
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


        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction BeginTransaction(IsolationLevel levl = IsolationLevel.ReadCommitted)
        {
            return transaction = _context.Database.BeginTransaction(levl);
        }


        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                _isTransactionRollback = true;
            }

        }

        public void Dispose()
        {
            if (transaction != null)
            {
                if (!_isTransactionCommited && !_isTransactionRollback)
                    transaction.Rollback();
                transaction.Dispose();
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
                    throw e;
                }
            }

            else
                value = await _context.SaveChangesAsync();

            return value;
        }
    }
}