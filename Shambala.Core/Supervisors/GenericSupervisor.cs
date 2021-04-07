using Shambala.Core.Contracts.Supervisors;
using AutoMapper;
using Shambala.Core.Contracts.Repositories;
namespace Shambala.Core.Supervisors
{
    public class GenericSupervisor<T, TDTO, V> : IGenericSupervisor<T, TDTO> where T : class where TDTO : class where V : IGenericRepository<T>
    {
        protected readonly IMapper _mapper;
        protected readonly V _repository;
        protected GenericSupervisor(IMapper mapper, V repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public TDTO Add(TDTO entityDTO)
        {
            T DomainEntity = _mapper.Map<T>(entityDTO);
            DomainEntity = _repository.Add(DomainEntity);
            return _mapper.Map<TDTO>(DomainEntity);
        }


        public TDTO GetById(int Id)
        {
            return _mapper.Map<TDTO>(_repository.GetById(Id));
        }

        public bool Update(TDTO entityDTO)
        {
            T DomainEntity = _mapper.Map<T>(entityDTO);
            return _repository.Update(DomainEntity);
        }
    }
}