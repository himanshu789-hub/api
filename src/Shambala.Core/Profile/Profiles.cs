using AutoMapper.Configuration.Conventions;
using AutoMapper.Configuration;
using Shambala.Domain;
using Shambala.Core.DTOModels;

namespace Shambala.Core.Profile
{
    public class ApplicationProfiles : AutoMapper.Profile
    {
        public ApplicationProfiles()
        {
            AddMemberConfiguration().AddName<PrePostfixName>(e => e.DestinationPostfixes.Contains("IdFkNavigation"));
            CreateMap<Salesman, SalesmanDTO>();
            CreateMap<Scheme, SchemeDTO>();
            CreateMap<OutgoingShipment, OutgoingShipmentDTO>();
            CreateMap<Shop, ShopDTO>();
            CreateMap<Shop, ShopWithInvoicesDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductFlavourQuantity, FlavourDTO>()
            .ForMember(destinationMember => destinationMember.Id, from => from.MapFrom(e => e.FlavourIdFk))
            .IncludeMembers(e => e.FlavourIdFkNavigation);
        }
    }
}