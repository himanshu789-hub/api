using Shambala.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Shambala.Configuration
{
    public static class OptionExtension
    {
        public static void AddServicesExtensionsWithIConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            var connection = configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionOptions>(options => connection.Bind(options));
        }
    }
}