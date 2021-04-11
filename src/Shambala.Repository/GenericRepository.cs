using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Shambala.Repository
{
  public  class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        ShambalaContext _context;
        public GenericRepository(ShambalaContext context) => _context = context;

        public T Add(T entity)
        {
            var AddedEntity = _context.Set<T>().Add(entity);
            return AddedEntity.Entity;
        }

        public T GetById(int Id)
        {
            return _context.Set<T>().Find("Id");
        }

        public bool Update(T entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
