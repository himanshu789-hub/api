using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
namespace Shambala.Controllers
{
    using Core.Contracts.Supervisors;
    using Core.Models.DTOModel;
    public class CreditController : ControllerBase
    {
        ICreditSupervisor creditSupervisor;
        public CreditController(ICreditSupervisor creditSupervisor) => this.creditSupervisor = creditSupervisor;

        [HttpPost]
        public IActionResult Add([FromBody] CreditDTO credit)
        {
            return Ok(creditSupervisor.Add(credit));
        }

        [HttpGet]
        public IActionResult GetLogs([FromQuery][BindRequired] int shipmentId, [FromQuery][BindRequired] short shopId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));

            return Ok(creditSupervisor.GetLog(shipmentId, shopId));
        }
        [HttpGet]
        public IActionResult IsCreditCleared([FromQuery][BindRequired] int shipmentId, [FromQuery][BindRequired] short shopId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(creditSupervisor.IsCreditCleared(shipmentId, shopId));
        }
        [HttpGet]
        public IActionResult GetLeftCredit([FromQuery][BindRequired] int shipmentId, [FromQuery][BindRequired] short shopId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(creditSupervisor.GetLeftOverCredit(shipmentId, shopId));
        }
    }
}