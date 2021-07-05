using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace Shambala.Controllers
{
    using Core.Models.DTOModel;
    using Shambala.Core.Supervisors;
    public class CreditController : ControllerBase
    {
        readonly ICreditSupervisor creditSupervisor;
        public CreditController(ICreditSupervisor creditSupervisor)
        {
            this.creditSupervisor = creditSupervisor;
        }
        [HttpGet]
        IActionResult GetLeftOverCredit(IEnumerable<ShopCreditOrDebitDTO> debits)
        {
            return Ok(creditSupervisor.GetLeftOverCredit(debits));
        }
    }
}