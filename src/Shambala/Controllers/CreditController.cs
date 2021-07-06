using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        [HttpPost]
        public IActionResult CheckLeftOver([FromBody][MinLength(1)] IEnumerable<ShopCreditOrDebitDTO> debits)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));

            return Ok(creditSupervisor.GetLeftOverCredit(debits));
        }
    }
}