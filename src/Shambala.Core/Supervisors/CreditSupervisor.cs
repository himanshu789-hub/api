using System.Collections.Generic;
using System.Linq;
namespace Shambala.Core.Supervisors
{
    using Contracts.Supervisors;
    using Contracts.Repositories;
    using Shambala.Core.Models.DTOModel;
    using Shambala.Core.Models.BLLModel;
    using Core.Helphers;
    public class CreditSupervisor : ICreditSupervisor
    {
        readonly IReadInvoiceRepository debitRead;
        public CreditSupervisor(IReadInvoiceRepository readRepository)
        {
            debitRead = readRepository;
        }

        public IEnumerable<ShopCreditOrDebitDTO> GetLeftOverCredit(IEnumerable<ShopCreditOrDebitDTO> debits)
        {
            throw new System.NotImplementedException();
        }
        // public IEnumerable<ShopCreditOrDebitDTO> GetLeftOverCredit(IEnumerable<ShopCreditOrDebitDTO> debits)
        // {
        //     IEnumerable<InvoiceAggreagateDetailBLL> result = debitRead.GetNotClearedAggregateByShopIds(debits.Select(e => e.ShopId).ToArray());
        //     IEnumerable<ShopCreditOrDebitDTO> creditOrDebitDTOs = Utility.CheckDebitUnderGivenBalance(debits,result);
        //     return creditOrDebitDTOs;
        // }
    }
}