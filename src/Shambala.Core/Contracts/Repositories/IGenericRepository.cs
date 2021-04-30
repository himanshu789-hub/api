using Shambala.Domain;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IGenericRepository<T>  
    {
        T Add(T entity);
        bool Update(T entity);
        T GetById(object Id);
    }
}