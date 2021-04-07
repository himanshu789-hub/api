using Shambala.Core.Contracts.Repositories;
using System.Threading.Tasks;

namespace Shambala.Core.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        IShopRepository ShopRepository { get; }
        IInvoiceRepository InvoiceRepository { get;  }
        IOutgoingShipmentRepository OutgoingShipmentRepository { get;  }
        IProductRepository ProductRepository { get;  }
        ISalesmanRepository SalesmanRepository { get;  }
        ISchemeRepository SchemeRepository { get;  }
        public void SaveChanges();
        void BeginTransaction();
        Task<int> SaveChangesAsync();
    }
}