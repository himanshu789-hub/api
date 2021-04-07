using Shambala.Infrastructure;
using Shambala.Core.Contracts.UnitOfWork;
using Shambala.Core.Contracts.Repositories;
using Shambala.Repository;
using System.Threading.Tasks;

namespace Shambala.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        ShambalaContext _context;
        IShopRepository _shopRepository{get;set;}
        IInvoiceRepository _invoiceRepository{get;set;}
        IOutgoingShipmentRepository _outgoingShipmentRepository{get;set;}
        IProductRepository  _productRepository{get;set;}
        ISalesmanRepository _salesRepository{get;set;}
        ISchemeRepository _schemeRepository{get;set;}
        public IShopRepository ShopRepository
        {
            get
            {
                return _shopRepository = _shopRepository == null ? new ShopRepository(_context) : ShopRepository;
            }
           
        }
        public  IInvoiceRepository InvoiceRepository
        {
            get
            {
                return _invoiceRepository =  _invoiceRepository == null ? new InvoiceRepository(_context) : InvoiceRepository;
            }
        }

        public  IOutgoingShipmentRepository OutgoingShipmentRepository
        {
            get
            {
                return _outgoingShipmentRepository =  _outgoingShipmentRepository == null ? new OutgoingShipmentRepository(_context) : OutgoingShipmentRepository;
            }
        }
        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository = _productRepository == null ? new ProductRepository(_context) : ProductRepository;
            }
        }

        public ISalesmanRepository SalesmanRepository
        {
            get
            {
                return _salesRepository =  _salesRepository == null ? new SalesmanRepository(_context) : SalesmanRepository;
            }
        }
        public ISchemeRepository SchemeRepository
        {
            get
            {
                return _schemeRepository =  _schemeRepository == null ? new SchemeRepository(_context) : SchemeRepository;
            }
        }

        public UnitOfWork(ShambalaContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            
            _context.SaveChanges();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }
    }
}