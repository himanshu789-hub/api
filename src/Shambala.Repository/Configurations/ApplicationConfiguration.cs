using Shambala.Core.Contracts.Repositories;
using Shambala.Repository;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationConfigurations
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<ISchemeRepository,SchemeRepository>();
            services.AddScoped<IInvoiceRepository,InvoiceRepository>();
            services.AddScoped<IIncomingShipmentRepository,IncomingShipmentRepository>();
            services.AddScoped<IOutgoingShipmentRepository,OutgoingShipmentRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<ISalesmanRepository,SalesmanRepository>();
            services.AddScoped<ISchemeRepository,SchemeRepository>();
            services.AddScoped<IShopRepository,ShopRepository>();
          
            services.AddScoped<IDebitReadRepository,ReadDebitRepository>();
            services.AddScoped<IReadInvoiceRepository,ReadInvoiceRepository>();
            services.AddScoped<IReadOutgoingShipmentRepository,ReadOutgoingShipmentRepository>();
        
            
            
            return services;
        }
        
    }
}