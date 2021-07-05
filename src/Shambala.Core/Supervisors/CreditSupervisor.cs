using System.Collections.Generic;
using System.Linq;
namespace Shambala.Core.Supervisors
{
    using Contracts.Supervisors;
    using Contracts.Repositories;
    using Shambala.Core.Models.DTOModel;
    using Shambala.Core.Models.BLLModel;
    public class CreditSupervisor : ICreditSupervisor
    {
        readonly IDebitReadRepository debitRead;
        public CreditSupervisor(IDebitReadRepository readRepository)
        {
            debitRead = readRepository;
        }
        public IEnumerable<ShopCreditOrDebitDTO> GetLeftOverCredit(IEnumerable<ShopCreditOrDebitDTO> debits)
        {
            IList<ShopCreditOrDebitDTO> creditOrDebitDTOs = new List<ShopCreditOrDebitDTO>();
            IEnumerable<InvoiceAggreagateDetailBLL> result = debitRead.GetLeftOverCreditByShopIds(debits.Select(e => e.ShopId).ToArray());
            foreach (var debit in debits)
            {
                if (result.Any(e => e.ShopId == debit.ShopId))
                {
                    decimal totalCrditAmount = result.Where(e => e.ShopId == debit.ShopId && !e.IsCleared).Sum(e => e.TotalPrice - e.TotalDueCleared);
                    if (totalCrditAmount < debit.Amount)
                    {
                        creditOrDebitDTOs.Add(new ShopCreditOrDebitDTO() { Amount = totalCrditAmount, ShopId = debit.ShopId });
                    }
                }
            }
            return creditOrDebitDTOs;
        }
    }
}