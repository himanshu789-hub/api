using Shambala.Domain;
using System.Collections.Generic;

namespace Shambala.Core.Contracts.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
    }
} 