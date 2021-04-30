using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Shambala.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        ShambalaContext _context;
        public GenericRepository(ShambalaContext context) => _context = context;
        public T Add(T entity)
        {
            var AddedEntity = _context.Set<T>().Add(entity);
            return AddedEntity.Entity;
        }

        public T GetById(object Id)
        {
            var EntityId = System.Convert.ChangeType(Id, typeof(T).GetProperty("Id").PropertyType);
            if (EntityId == null)
                throw new System.Exception("Id cannot be converted to type '"+typeof(T).GetType().GetProperty("Id").PropertyType.FullName+"'");
            var Entity = _context.Set<T>().Find(EntityId);
            return Entity;
        }

        public bool Update(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
