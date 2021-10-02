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
            customCaratPrice.Id = 0;
            return this.context.CustomCaratPrice.Add(customCaratPrice).Entity;
        }

        public bool Delete(int Id)
        {
            this.context.CustomCaratPrice.Remove(new CustomCaratPrice(){Id = Id});
            return true;
        }
    }
}