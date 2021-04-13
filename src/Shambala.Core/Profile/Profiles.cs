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
            AddMemberConfiguration().AddName<PrePostfixName>(e => e.AddStrings(e => e.DestinationPostfixes, "Fk", "IdFkNavigation"));


            CreateMap<Salesman, SalesmanDTO>();
            CreateMap<Salesman, SalesmanDTO>().ReverseMap();

            CreateMap<Scheme, SchemeDTO>();
            CreateMap<Salesman, SalesmanDTO>().ReverseMap();

            CreateMap<OutgoingShipment, OutgoingShipmentDTO>();
            CreateMap<OutgoingShipment, OutgoingShipmentDTO>().ReverseMap();


            CreateMap<IncomingShipment, IncomingShipmentDTO>();
            CreateMap<IncomingShipment, IncomingShipmentDTO>().ReverseMap();

            CreateMap<Shop, ShopDTO>();
            CreateMap<Shop, ShopDTO>().ReverseMap();

            CreateMap<Shop, ShopWithInvoicesDTO>();
            CreateMap<Shop, ShopWithInvoicesDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>();
            CreateMap<Product, ProductDTO>().ReverseMap();

            CreateMap<ProductFlavourQuantity, FlavourDTO>(AutoMapper.MemberList.None)
            .ForMember(destinationMember => destinationMember.Id, from => from.MapFrom(e => e.FlavourIdFk))
            .ForMember(e => e.Quantity, map => map.MapFrom(e => e.Quantity))
            .ForMember(e => e.Title, map => map.MapFrom(e => e.FlavourIdFkNavigation.Title));

            CreateMap<ProductFlavourQuantity, FlavourDTO>(AutoMapper.MemberList.None)
            .ForMember(destinationMember => destinationMember.Id, from => from.MapFrom(e => e.FlavourIdFk))
            .ForMember(e => e.Quantity, map => map.MapFrom(e => e.Quantity))
            .ForMember(e => e.Title, map => map.MapFrom(e => e.FlavourIdFkNavigation.Title)).ReverseMap();

        }
    }
}