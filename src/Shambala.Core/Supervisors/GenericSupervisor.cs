using Shambala.Core.Contracts.Supervisors;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Shambala.Core.Contracts.Repositories;
namespace Shambala.Core.Supervisors
{
    public interface IQuerySupervisor<T> where T : class
    {
        bool IsNameAlreadyExists(string name, int? Id);
        IEnumerable<T> GetAllByName(string name);
    }
    public class GenericSupervisor<T, TDTO, V> : IGenericSupervisor<TDTO> where T : class where TDTO : class where V : IGenericRepository<T>
    {
        protected readonly IMapper _mapper;
        protected readonly V _repository;
        // protected readonly ILogger<U> _logger;

        protected GenericSupervisor(IMapper mapper, V repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public TDTO Add(TDTO entityDTO)
        {
            T DomainEntity = _mapper.Map<T>(entityDTO);
            DomainEntity = _repository.Add(DomainEntity);
            _repository.SaveChanges();
            return _mapper.Map<TDTO>(DomainEntity);
        }

        public TDTO GetById(object Id)
        {
            return _mapper.Map<TDTO>(_repository.GetById(Id));
        }
        public bool Update(TDTO entityDTO)
        {
            T DomainEntity = _mapper.Map<T>(entityDTO);
            bool IsUpdated = _repository.Update(DomainEntity);
            if (IsUpdated)
            {
                _repository.SaveChanges();
            }
            return IsUpdated;
        }
    }
}