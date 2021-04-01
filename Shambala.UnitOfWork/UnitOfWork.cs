using Shambala.Infrastructure;
using Shambala.Core.Contracts.UnitOfWork;

namespace Shambala.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        ShambalaContext _context;
        IShopRepository ShopRepository
        {
            get
            {
                ShopRepository == null ? new ShopRepository(_context) : ShopRepository;
            }
        }
        IInvoiceRepository InvoiceRepository
        {
            get
            {
                InvoiceRepository == null ? new InvoiceRepository(_context) : InvoiceRepository;
            }
        }
        
        IOutgoingShipmentRepository OutgoingShipmentRepository
        {
            get
            {
                OutgoingShipmentRepository == null ? new OutgoingShipmentRepository(_context) : OutgoingShipmentRepository;
            }
        }
        IProductRepository ProductRepository
        {
            get
            {
                ProductRepository == null ? new ProductRepository(_context) : ProductRepository;
            }
        }
        
        ISalesmanRepository SalesmanRepository
        {
            get
            {
                SalesmanRepository == null ? new SalesmanRepository(_context) : SalesmanRepository;
            }
        }
        ISchemeRepository SchemeRepository
        {
            get
            {
                SchemeRepository == null ? new SchemeRepository(_context) : SchemeRepository;
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
        public Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}