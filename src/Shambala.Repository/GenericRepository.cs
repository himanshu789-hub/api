using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Shambala.Domain;
using System.Collections.Generic;

namespace Shambala.Repository
{
    public class GenericLoading<T> : ILoadingProperties<T> where T : class
    {
        readonly ShambalaContext _context;
        public GenericLoading(ShambalaContext context) => _context = context;
        public void Load<TProperty>(T entity, System.Linq.Expressions.Expression<System.Func<T, TProperty>> expression) where TProperty : class
        {
            _context.Entry(entity).Reference(expression).Load();
        }
    }
    public class GenericRepository<T> : GenericLoading<T>, IGenericRepository<T> where T : class
    {
        ShambalaContext _context;
        public GenericRepository(ShambalaContext context) : base(context) => _context = context;
        public T Add(T entity)
        {
            if (typeof(T).GetProperty("IsActive") != null && typeof(T).GetProperty("IsActive").PropertyType.FullName == typeof(bool).FullName)
            {
                typeof(T).GetProperty("IsActive").SetValue(entity, true);
            }
            var AddedEntity = _context.Set<T>().Add(entity);

            return AddedEntity.Entity;
        }

        public IEnumerable<T> FetchList(System.Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public T GetById(object Id)
        {
            var EntityId = System.Convert.ChangeType(Id, typeof(T).GetProperty("Id").PropertyType);
            if (EntityId == null)
                throw new System.Exception("Id cannot be converted to type '" + typeof(T).GetType().GetProperty("Id").PropertyType.FullName + "'");
            var Entity = _context.Set<T>().Find(EntityId);
            return Entity;
        }


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public Task<int> SaveChangesAync()
        {
            return _context.SaveChangesAsync();
        }

        public bool Update(T entity)
        {
            if (typeof(T).GetProperty("IsActive") != null && typeof(T).GetProperty("IsActive").PropertyType.FullName == typeof(bool).FullName)
            {
                typeof(T).GetProperty("IsActive").SetValue(entity, true);
            }
            if (typeof(T).GetProperty("Id") == null)
                throw new System.Exception("Id Property Not Found");

            _context.Set<T>().Update(entity);
            return true;

        }

    }
}
