using Shambala.Domain;

namespace Shambala.Core.Contracts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
    }
} 