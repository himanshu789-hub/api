

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
        static public IEnumerable<int> CheckDebitUnderGivenBalance(IEnumerable<ShopCreditOrDebitDTO> recieveBalances, IEnumerable<LeftOverCreditBLL> leftOverCreditBLLs)
        {
            ICollection<int> ShopIds = new List<int>();
            foreach (var recieveBalance in recieveBalances)
            {
                decimal totalCredit = leftOverCreditBLLs.Where(e => e.ShopId == recieveBalance.ShopId).Sum(e => e.Credit);
                if (recieveBalance.Amount > totalCredit)
                    ShopIds.Add(recieveBalance.ShopId);
            }
            return ShopIds;
        }
        
    }
}