

namespace Shambala.Core.Contracts.Repositories
{
    public class IGenericRepository<T> where T : class
    {
        T Add(T entity);
        boolean Delete(int Id);
        T Get(int Id);
        boolean Update(T entity);
    }
}