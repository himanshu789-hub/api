

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
            foreach (var receieveDebt in recieveBalances)
            {
                decimal totalCreditAmount = leftOverCreditBLLs.Where(e => e.ShopId == receieveDebt.ShopId && !e.IsCleared).Sum(e => e.TotalPrice - e.TotalDueCleared);
                if (totalCreditAmount < receieveDebt.Amount)
                {
                    leftOverCredit.Add(new ShopCreditOrDebitDTO() { Amount = totalCreditAmount, ShopId = receieveDebt.ShopId });
                }
            }
            return leftOverCredit;
        }

    }
}