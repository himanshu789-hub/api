using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shambala.Configurations;
namespace Shambala
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        string CorsPolicy = "AllowAllOrigin";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMappingConfiguration()
            .AddSupervisorServices()
            .AddRepositoryServices()
            .AddConnectionOptionServices(Configuration)
            .ConfigureSQLInstance(Configuration)
            .AddUnitOfWorkService()
            .AddSchemeConfig(Configuration);

            services.AddCors(options => options.AddPolicy(CorsPolicy, policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            services.AddSpaStaticFiles(spa => spa.RootPath = "ClientApp/build");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id:int?}"
                );
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../../shambala_ui";
                if (env.IsDevelopment())
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
            });
        }
    }
}
