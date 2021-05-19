using System.Collections.Generic;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface IGenericSupervisor<TDTO> where TDTO : class
    {
        TDTO Add(TDTO entityDTO);
        bool Update(TDTO entityDTO);
        TDTO GetById(object Id);
    }
}