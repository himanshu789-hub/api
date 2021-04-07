using AutoMapper;
using Shambala.Domain;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Supervisors
{
    public class ApplicationProfiles : Profile
    {
        public ApplicationProfiles()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Salesman, SalesmanDTO>();
                cfg.CreateMap<Scheme, SchemeDTO>();
            });

        }
    }
}