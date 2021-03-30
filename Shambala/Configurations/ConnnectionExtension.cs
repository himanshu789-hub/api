using Shambala.Infrastructure;

namespace Shambala.Configurations
{
    public static class ConnectionExtension
    {
        public IServiceCollection ConfigureSQLInstance(IServiceCollection services)
        {
             services.AddDbContext<ShamabalaContext>(options =>
             options.UseMySQL(Configuration.GetConnectionString("MySQLConnection")));

            return services;
        }
        
    }
}