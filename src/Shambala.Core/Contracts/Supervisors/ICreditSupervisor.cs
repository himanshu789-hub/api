
using System.Collections.Generic;
namespace Shambala.Core.Supervisors
{
    using Shambala.Core.Models.BLLModel;
    using Shambala.Core.Models.DTOModel;
    
    public interface ICreditSupervisor
    {
     IEnumerable<ShopCreditOrDebitDTO> GetLeftOverCredit(IEnumerable<ShopCreditOrDebitDTO> debits);
    }
}