using Shambala.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shambala.Configurations
{
    public static class ConnectionExtension
    {
        public static IServiceCollection ConfigureSQLInstance(IServiceCollection services,IConfiguration configuration)
        {
             services.AddDbContext<ShambalaContext>(options =>
             options.UseMySQL(configuration.GetConnectionString("MySQLConnection")));
            return services;
        }
        
    }
}