namespace Shambala.Repository
{
    using Core.Contracts.Repositories;
    using Shambala.Domain;
    using Shambala.Infrastructure;
    public class CustomPriceRepository : ICustomPriceRepository
    {
        readonly ShambalaContext context;
        public CustomPriceRepository(ShambalaContext shambalaContext) => this.context = shambalaContext;
        public CustomCaratPrice Add(CustomCaratPrice customCaratPrice)
        {
            return this.context.CustomCaratPrice.Add(customCaratPrice).Entity;
        }

        public bool Delete(CustomCaratPrice customCaratPrice)
        {
            this.context.CustomCaratPrice.Remove(customCaratPrice);
            return true;
        }
    }
}