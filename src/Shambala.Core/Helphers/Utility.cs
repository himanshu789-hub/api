

using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace Shambala.Core.Helphers
{
    using Domain;
    using Models.DTOModel;
    using Models.BLLModel;
    public class Utility
    {
        static public bool IsDueCompleted(decimal DuePrice)
        {
            return DuePrice == 0;
        }
        static public IEnumerable<ShopCreditOrDebitDTO> CheckDebitUnderGivenBalance(IEnumerable<ShopCreditOrDebitDTO> recieveBalances, IEnumerable<InvoiceAggreagateDetailBLL> leftOverCreditBLLs)
        {
            IList<ShopCreditOrDebitDTO> leftOverCredit = new List<ShopCreditOrDebitDTO>();
            foreach (var debit in recieveBalances)
            {
                if (leftOverCreditBLLs.Any(e => e.ShopId == debit.ShopId))
                {
                    decimal totalCreditAmount = leftOverCreditBLLs.Where(e => e.ShopId == debit.ShopId && !e.IsCleared).Sum(e => e.TotalPrice - e.TotalDueCleared);
                    if (totalCreditAmount < debit.Amount)
                    {
                        leftOverCredit.Add(new ShopCreditOrDebitDTO() { Amount = totalCreditAmount, ShopId = debit.ShopId });
                    }
                }
            }
            return leftOverCredit;
        }

    }
}