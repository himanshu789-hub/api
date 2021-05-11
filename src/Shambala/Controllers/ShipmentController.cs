using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
using System.Data;
using System.Linq;
using Shambala.Core.Exception;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Shambala.Controllers
{
    using Core.Helphers;
    public class ShipmentController : ControllerBase
    {
        IOutgoingShipmentSupervisor _outgoingSupervisor;
        public ShipmentController(IOutgoingShipmentSupervisor outgoingShipmentSupervisor)
        {
            _outgoingSupervisor = outgoingShipmentSupervisor;
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PostOutgoingShipmentDTO postOutgoing)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(await _outgoingSupervisor.AddAsync(postOutgoing));
        }

        [HttpPut]
        public async Task<IActionResult> ReturnAsync([FromRoute] int Id, [FromBody] IEnumerable<ShipmentDTO> shipmentDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.Select(e => e.Errors.Select(e => e.ErrorMessage)));
            try
            {
                await _outgoingSupervisor.ReturnAsync(Id, shipmentDTOs);
                return Ok();
            }
            catch (System.Exception e)
            {
                if (e is DuplicateShipmentsException || e is OutgoingShipmentNotOperableException)
                    return BadRequest(e.Message);
                else
                    throw;
            }
        }
        public IActionResult GetProductListByOrderId([FromRoute] int orderId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(_outgoingSupervisor.GetProductListByOrderId(orderId));
        }

        [HttpPost]
        public async Task<IActionResult> CompleteAsync([FromRoute][Required] int Id, [FromBody] IEnumerable<PostInvoiceDTO> postInvoiceDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            try
            {
                return Ok(await _outgoingSupervisor.CompleteAsync(Id, Utility.ToInvoices(postInvoiceDTOs)));
            }
            catch (System.Exception e)
            {
                if (e is OutgoingShipmentNotOperableException || e is DuplicateNameException)
                    return BadRequest(e.Message);
                throw;
            }
        }
        [HttpGet]
        public IActionResult GetOutgoingBySalesmanIdAndDate([FromQuery] short salesmanId, [FromQuery] System.DateTime date)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.Select(e => e.ErrorMessage)));
            return Ok(_outgoingSupervisor.GetOutgoingShipmentBySalesmanIdAndAfterDate(salesmanId, date));
        }
    }
}