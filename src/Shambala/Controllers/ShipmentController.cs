using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace Shambala.Controllers
{
    using Shambala.Core.Models.DTOModel;
    using Shambala.Core.Models;
    using Shambala.Core.Helphers;
    public class ShipmentController : ControllerBase
    {
        IOutgoingShipmentSupervisor _outgoingSupervisor;
        IReadOutgoingSupervisor _outgoingReadSupervisor;
        public ShipmentController(IOutgoingShipmentSupervisor outgoingShipmentSupervisor, IReadOutgoingSupervisor readOutgoingSupervisor)
        {
            _outgoingSupervisor = outgoingShipmentSupervisor;
        this._outgoingReadSupervisor = readOutgoingSupervisor;
        }
        [HttpGet]
        public IActionResult GetById([FromQuery][BindRequired] int Id)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(_outgoingSupervisor.GetById(Id));
        }

        [HttpPost]
        public IActionResult Add([FromBody] OutgoingShipmentPostDTO postOutgoing)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            ResultModel resultModel = _outgoingSupervisor.AddAsync(postOutgoing);
            if (resultModel.IsValid)
                return Ok(resultModel.Content);

            return BadRequest(resultModel);
        }

        [HttpPut]
        public IActionResult Update([FromBody] OutgoingShipmentDTO outgoingShipment)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e => e.Errors));

            ResultModel resultModel = _outgoingSupervisor.Update(outgoingShipment);
            if (!resultModel.IsValid)
            {
                if (resultModel.Code == ((int)ConcurrencyErrorCode.Concurrency_Error))
                    return UnprocessableEntity(resultModel);
                return BadRequest(resultModel);
            }
            return Ok(resultModel.Content);
        }
        [HttpGet]
        public IActionResult GetOutgoingBySalesmanIdAndDate([FromQuery][BindRequired] short salesmanId, [FromQuery][BindRequired] System.DateTime date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(_outgoingReadSupervisor.GetOutgoingShipmentBySalesmanIdAndAfterDate(salesmanId, date));
        }
        public IActionResult GetDetailsById([FromRoute][BindRequired] int Id)
        {
            return Ok(_outgoingReadSupervisor.GetAggregate(Id));
        }
    }
}