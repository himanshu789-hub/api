using Shambala.Domain;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace Shambala.Core.Contracts.Repositories
{
    public interface ILoadingProperties<T> where T :class 
    {
        void Load<TProperty>(T entity, Expression<System.Func<T,TProperty>> expression) where TProperty : class;
    }
    public interface IGenericRepository<T>  :ILoadingProperties<T> where T : class
    {
        T Add(T entity);
        bool Update(T entity);
        T GetById(object Id);
        int SaveChanges();
        Task<int> SaveChangesAync();
    }
}