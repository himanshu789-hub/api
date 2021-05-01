using Shambala.Domain;
using System.Threading.Tasks;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IGenericRepository<T>  
    {
        T Add(T entity);
        bool Update(T entity);
        T GetById(object Id);
        int SaveChanges();
        Task<int> SaveChangesAync();
    }
}