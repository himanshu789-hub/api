using Shambala.Core.Contracts.Repositories;
using Shambala.Repository;
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationConfigurations
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddScoped<ISchemeRepository,SchemeRepository>();
            return services;
        }
        
    }
}