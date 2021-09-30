using AutoMapper;
using Shambala.Core.Profile;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.Supervisors;
using Microsoft.Extensions.Configuration;
namespace Microsoft.Extensions.DependencyInjection
{
    using Shambala.Core.Helphers;
    
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
            services.AddScoped<IDebitSupervisor, DebitSupervisor>();
            services.AddScoped<ISalesmanSupervisor, SalesmanSupervisor>();
            services.AddScoped<IIncomingShipmentSupervisor, IncomingShipmentSupervisor>();
            services.AddScoped<IInvoiceSupervisor, InvoiceSupervisor>();
            services.AddScoped<IShopSupervisor, ShopSupervisor>();
            services.AddScoped<ICreditSupervisor, CreditSupervisor>();
            services.AddScoped<IReadOutgoingSupervisor,ReadOutgoingSupervisor>();

            return services;
        }
        public static IServiceCollection AddSchemeConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<SchemeProductOptions>(options=>configuration.GetSection(SchemeProductOptions.Option).Bind(options));
            return services;
        }
    }
}