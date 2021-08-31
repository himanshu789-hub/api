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
        // [HttpPut]
        // public async Task<IActionResult> ReturnAsync([FromRoute] int Id, [FromBody] IEnumerable<ShipmentDTO> shipmentDTOs)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState.Values.Select(e => e.Errors.Select(e => e.ErrorMessage)));
        //     try
        //     {
        //         await _outgoingSupervisor.ReturnShipmentAsync(Id, shipmentDTOs);
        //         return Ok();
        //     }
        //     catch (System.Exception e)
        //     {
        //         if (e is DuplicateShipmentsException || e is OutgoingShipmentNotOperableException)
        //             return BadRequest(new BadRequestErrorModel() { Code = (int)OutgoingBadErrorCodes.DUPLICATE, Message = e.Message });
        //         else
        //             throw;
        //     }
        // }
        //[HttpPost]
        // public async Task<IActionResult> CompleteAsync([Required][FromBody] ShipmentLedgerDetail shipmentLedgerDetail)
        // {

        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
        //     try
        //     {
        //         return Ok(await _outgoingSupervisor.CompleteAsync(shipmentLedgerDetail));
        //     }
        //     catch (System.Exception e)
        //     {
        //         if (e is OutgoingShipmentNotOperableException)
        //             return BadRequest(new BadRequestErrorModel() { Message = e.Message, Code = (int)OutgoingBadErrorCodes.OUTGOINGSHIPMENT_NOT_OPERABLE });
        //         if (e is DuplicateNameException)
        //             return BadRequest(new BadRequestErrorModel() { Message = e.Message, Code = (int)OutgoingBadErrorCodes.DUPLICATE_SHIPMENT });
        //         throw;
        //     }
        // }
        [HttpGet]
        public IActionResult GetOutgoingBySalesmanIdAndDate([FromQuery][BindRequired] short salesmanId, [FromQuery][BindRequired] System.DateTime date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(_outgoingSupervisor.GetOutgoingShipmentBySalesmanIdAndAfterDate(salesmanId, date));
        }

    }
}