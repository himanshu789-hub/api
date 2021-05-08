using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shambala.Infrastructure;
namespace Microsoft.Extensions.DependencyInjection
{
 
    public static class ConnectionExtension
    {
        public static IServiceCollection ConfigureSQLInstance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShambalaContext>(options =>
            options.UseMySQL(configuration.GetConnectionString("MySQLConnection")));
            return services;
        }

    }
}