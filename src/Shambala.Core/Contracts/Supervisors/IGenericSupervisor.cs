namespace Shambala.Core.Contracts.Supervisors
{
    public interface IGenericSupervisor<T,TDTO> where T : class where TDTO : class
    {
         TDTO Add(TDTO entityDTO);
         bool Update(TDTO entityDTO);
         TDTO GetById(int Id);
    }
}