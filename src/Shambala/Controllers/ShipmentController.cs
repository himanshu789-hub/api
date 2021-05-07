using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.DTOModels;
using System.Collections.Generic;
using System.Linq;
using Shambala.Core.Exception;
namespace Shambala.Controllers
{
    public class ShipmentController : ControllerBase
    {
        IOutgoingShipmentSupervisor _outgoingSupervisor;
        public ShipmentController(IOutgoingShipmentSupervisor outgoingShipmentSupervisor)
        {
            _outgoingSupervisor = outgoingShipmentSupervisor;
        }
        public async Task<IActionResult> AddAsync(PostOutgoingShipmentDTO postOutgoing)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(await _outgoingSupervisor.AddAsync(postOutgoing));

        }
        public async Task<IActionResult> ReturnAsync(OutgoingShipmentDTO outgoingShipmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors));
            try
            {
                await _outgoingSupervisor.ReturnAsync(outgoingShipmentDTO);
            }
            catch (System.Exception e)
            {
                if (e is DuplicateShipmentsException || e is OutgoingShipmentNotOperableException)
                    return BadRequest(e.Message);
                else
                    throw e;
            }
            return Ok();
        }
        public IActionResult GetProductListByOrderId([FromRoute] int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(_outgoingSupervisor.GetProductListByOrderId(orderId));
        }
    }
}