using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.DTOModels;
using System.Collections.Generic;

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
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Root.Errors);

            bool IsAdded = await _outgoingSupervisor.AddAsync(postOutgoing);
            if (IsAdded)
                return Ok();
            else
                return BadRequest();
        }
        public IActionResult GetProductListByOrderId([FromRoute] int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Root.Errors);

            return Ok(_outgoingSupervisor.GetProductListByOrderId(orderId));
        }
    }
}