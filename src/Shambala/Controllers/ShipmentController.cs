using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using Shambala.Core.Exception;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shambala.Controllers
{
    using Shambala.Core.Models.DTOModel;
    using Shambala.Core.Models;
    using Core.Helphers;
    public class ShipmentController : ControllerBase
    {
        IOutgoingShipmentSupervisor _outgoingSupervisor;
        public ShipmentController(IOutgoingShipmentSupervisor outgoingShipmentSupervisor)
        {
            _outgoingSupervisor = outgoingShipmentSupervisor;
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

            return UnprocessableEntity(resultModel);
        }

        [HttpPut]
        public IActionResult Update([FromBody] OutgoingShipmentDTO outgoingShipment)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e => e.Errors));

            ResultModel resultModel = _outgoingSupervisor.Update(outgoingShipment);
            if (!resultModel.IsValid)
                return UnprocessableEntity(resultModel);
            return Ok(resultModel.Content);
        }
        [HttpGet]
        public IActionResult GetOutgoingBySalesmanIdAndDate([FromQuery][BindRequired] short salesmanId, [FromQuery][BindRequired] System.DateTime date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(_outgoingSupervisor.GetOutgoingShipmentBySalesmanIdAndAfterDate(salesmanId, date));
        }
    }
}