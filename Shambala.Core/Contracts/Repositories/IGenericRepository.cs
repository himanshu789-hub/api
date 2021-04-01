
namespace Shambala.Core.Contracts.Repositories
{
    public interface IGenericRepository<T>  where T : class
    {
        T Add(T entity);
        bool Update(T entity);
    }
}