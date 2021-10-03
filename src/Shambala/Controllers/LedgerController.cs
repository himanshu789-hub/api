using System.Linq;
using Microsoft.AspNetCore.Mvc;
namespace Shambala.Controllers
{
    using Core.Contracts.Supervisors;
    using Core.Models.DTOModel;
    using Core.Models;
    using Core.Helphers;
    public class LedgerController : ControllerBase
    {
        ILedgerSupervisor ledgerSupervisor;
        public LedgerController(ILedgerSupervisor supervisor)
        {
            ledgerSupervisor = supervisor;
        }
        [HttpPost]
        public IActionResult Add([FromBody]LedgerDTO ledger)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
            }
            ResultModel resultModel = ledgerSupervisor.Post(ledger);
            if (!resultModel.IsValid)
            {
                if (resultModel.Code == ((int)ConcurrencyErrorCode.Concurrency_Error))
                    return UnprocessableEntity(resultModel);
                return BadRequest(resultModel);
            }
            return Ok();
        }
    }
}