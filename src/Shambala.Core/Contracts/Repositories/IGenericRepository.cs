using Shambala.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace Shambala.Core.Contracts.Repositories
{
    public interface IQueryList<T> where T : class
    {
  //      bool IsNameAlreadyExists(string name, int? Id);
    }
    public interface ILoadingProperties<T> where T : class
    {
        void Load<TProperty>(T entity, Expression<System.Func<T, TProperty>> expression) where TProperty : class;
    }
    public interface IGenericRepository<T> : ILoadingProperties<T> where T : class
    {
        T Add(T entity);
        bool Update(T entity);
        T GetById(object Id);
        IEnumerable<T> FetchList(System.Func<T, bool> predicate);
        int SaveChanges();
        Task<int> SaveChangesAync();
    }
}