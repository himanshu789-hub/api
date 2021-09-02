using AutoMapper.Configuration.Conventions;
using AutoMapper.Configuration;
using AutoMapper.QueryableExtensions;
using Shambala.Domain;
using Shambala.Core.Models.DTOModel;
using AutoMapper;
namespace Shambala.Core.Profile
{
    using AutoMapper;
    using Models;
    using Helphers;
    public class ApplicationProfiles : AutoMapper.Profile
    {
        class StringToOutgoingEnum : AutoMapper.ITypeConverter<string, OutgoingShipmentStatus>
        {
            public OutgoingShipmentStatus Convert(string source, OutgoingShipmentStatus destination, ResolutionContext context)
            {
                OutgoingShipmentStatus result;
                if (System.Enum.TryParse(source, out result))
                    return result;

                return OutgoingShipmentStatus.PENDING;

            }
        }
        public ApplicationProfiles()
        {

            AddMemberConfiguration()
            .AddName<PrePostfixName>(e => e.AddStrings(p => p.DestinationPostfixes, "Fk", "IdFkNavigation"));


            AddMemberConfiguration()
            .AddName<PrePostfixName>(e => e.AddStrings(p => p.Postfixes, "Fk", "IdFkNavigation"));


            CreateMap<Salesman, SalesmanDTO>();
            CreateMap<Salesman, SalesmanDTO>().ReverseMap();

            CreateMap<Scheme, SchemeDTO>();
            CreateMap<Scheme, SchemeDTO>().ReverseMap();

            CreateMap<Debit, DebitDTO>();
            CreateMap<Debit, DebitDTO>().ReverseMap();

            //            CreateMap<InvoiceAggreagateDetailBLL, InvoiceDetailDTO>();

            CreateMap<OutgoingShipmentStatus, string>()
            .ConvertUsing(src => System.Enum.GetName(typeof(OutgoingShipmentStatus), src));
            CreateMap<string, OutgoingShipmentStatus>().ConvertUsing<StringToOutgoingEnum>();

            CreateMap<OutgoingShipmentDetails, OutgoingShipmentDetailDTO>()
            .ForPath(e => e.SchemeInfo.TotalSchemePrice, map => map.MapFrom(e => e.SchemeTotalPrice))
            .ForPath(e => e.SchemeInfo.TotalQuantity, m => m.MapFrom(e => e.SchemeTotalQuantity))
            .ForPath(e => e.SchemeInfo.SchemeQuantity, m => m.MapFrom(e => Utility.GetSchemeQuantityPerCaret(e.TotalQuantityShiped, e.SchemeTotalQuantity, e.CaretSize)));


            CreateMap<OutgoingShipmentDetails, OutgoingShipmentDetailDTO>()
            .ForPath(e => e.SchemeInfo.TotalSchemePrice, map => map.MapFrom(e => e.SchemeTotalPrice))
            .ForPath(e => e.SchemeInfo.TotalQuantity, m => m.MapFrom(e => e.SchemeTotalQuantity))
            .ReverseMap();


            CreateMap<CustomCaratPrice, CustomCaratPriceDTO>();
            CreateMap<CustomCaratPrice, CustomCaratPriceDTO>()
            .ReverseMap()
            .ForMember(e => e.OutgoinShipmentDetailIdFk, m => m.MapFrom((src, desr, d, context) => context.Items["OutgoingId"] == null ? 0 : context.Items["Id"]));




            // CreateMap<OutgoingShipmentDetails, ShipmentDTO>()
            // .ForMember(e => e.TotalRecievedPieces, map => map.MapFrom(e => e.TotalQuantityShiped))
            // .ForMember(e => e.TotalDefectPieces, map => map.MapFrom(e => e.TotalQuantityRejected));

            // CreateMap<OutgoingShipmentDetails, ShipmentDTO>()
            // .ForMember(e => e.TotalRecievedPieces, map => map.MapFrom(e => e.TotalQuantityShiped))
            // .ForMember(e => e.TotalDefectPieces, map => map.MapFrom(e => e.TotalQuantityRejected))
            // .ReverseMap();

            CreateMap<OutgoingShipment, OutgoingShipmentInfoDTO>();
            CreateMap<OutgoingShipment, OutgoingShipmentInfoDTO>()
            .ReverseMap();
            
            CreateMap<OutgoingShipment,OutgoingShipmentDTO>().ReverseMap();
            
            CreateMap<OutgoingShipment, OutgoingShipmentPostDTO>()
            .ForMember(e => e.Shipments, map => map.MapFrom(e => e.OutgoingShipmentDetails))
            .ReverseMap();


            CreateMap<IncomingShipment, ShipmentDTO>();
            CreateMap<IncomingShipment, ShipmentDTO>().ReverseMap();

            CreateMap<Shop, ShopDTO>();
            CreateMap<Shop, ShopDTO>().ReverseMap();

            CreateMap<Shop, ShopInfoDTO>();

            CreateMap<Shop, ShopWithInvoicesDTO>();
            CreateMap<Shop, ShopWithInvoicesDTO>().ReverseMap();

            CreateMap<Product, ProductDTO>()
            .ForMember(e => e.Flavours, map => map.MapFrom(e => e.ProductFlavourQuantity));
            CreateMap<Product, ProductDTO>()
            .ForMember(e => e.Flavours, map => map.MapFrom(e => e.ProductFlavourQuantity)).ReverseMap();

            CreateMap<ProductFlavourQuantity, FlavourDTO>(AutoMapper.MemberList.None)
            .ForMember(destinationMember => destinationMember.Id, from => from.MapFrom(e => e.FlavourIdFk))
            .ForMember(e => e.Quantity, map => map.MapFrom(e => e.Quantity))
            .ForMember(e => e.Title, map => map.MapFrom(e => e.FlavourIdFkNavigation.Title));

            CreateMap<ProductFlavourQuantity, FlavourDTO>(AutoMapper.MemberList.None)
            .ForMember(destinationMember => destinationMember.Id, from => from.MapFrom(e => e.FlavourIdFk))
            .ForMember(e => e.Title, map => map.MapFrom(e => e.FlavourIdFkNavigation.Title))
            .ForMember(e => e.Quantity, map => map.MapFrom(e => e.Quantity))
            .ForMember(e => e.Title, map => map.MapFrom(e => e.FlavourIdFkNavigation.Title)).ReverseMap();


            // CreateMap<InvoiceAggreagateDetailBLL, InvoiceDetailWithInfoBLL>();
            // CreateMap<InvoiceDetailWithInfoBLL, InvoicewithCreditLogBLL>();
            // CreateMap<InvoiceDetailWithInfoBLL, ShopBillInfo>();
            // CreateMap<InvoicewithCreditLogBLL, InvoicewithCreditLogDTO>();
            // CreateMap<ShopBillInfo, InvoiceBillDTO>()
            // .ForMember(e => e.BillingInfo, map => map.MapFrom(e => e.BillingInfoBLLs));

            // CreateMap<LedgerWithPastDebitDTO, ShopCreditOrDebitDTO>()
            // .ForMember(e => e.Amount, map => map.MapFrom(e => e.OldDebit));


        }
    }
}