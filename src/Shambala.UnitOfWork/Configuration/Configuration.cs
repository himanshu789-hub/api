namespace Microsoft.Extensions.DependencyInjection
{
    public static class UnitOfWorkServices
    {
        public static IServiceCollection AddUnitOfWorkService(this IServiceCollection services)

        {
            services.AddScoped<Shambala.Core.Contracts.UnitOfWork.IUnitOfWork, Shambala.UnitOfWork.UnitOfWork>();
            return services;
        }
    }

}