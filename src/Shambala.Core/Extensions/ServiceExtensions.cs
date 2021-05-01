using AutoMapper;
using Shambala.Core.Profile;
using Microsoft.Extensions.DependencyInjection;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.Supervisors;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new ApplicationProfiles()));

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
        public static IServiceCollection AddSupervisorServices(this IServiceCollection services)
        {
            services.AddScoped<IProductSupervisor, ProductSupervisor>();
            services.AddScoped<IOutgoingShipmentSupervisor, OutgoingShipmentSupervisor>();
            services.AddScoped<ISchemeSupervisor, SchemeSupervisor>();
            services.AddScoped<ISalesmanSupervisor, SalesmanSupervisor>();
            services.AddScoped<IIncomingShipmentSupervisor, IncomingShipmentSupervisor>();
            services.AddScoped<IInvoiceSupervisor, InvoiceSupervisor>();
            services.AddScoped<IShopSupervisor, ShopSupervisor>();
            return services;
        }
    }
}