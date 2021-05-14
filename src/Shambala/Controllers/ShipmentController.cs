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
    using Models;
    using Core.Models.DTOModel;
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

            OutgoingShipmentWithSalesmanInfoDTO outgoing = await _outgoingSupervisor.AddAsync(postOutgoing);

            if (outgoing == null)
                return BadRequest(new BadRequestErrorModel() { Code = (int)OutgoingBadErrorCodes.QUANTITIES_OUT_OF_STOCK, Model = _outgoingSupervisor.ProvideOutOfStockQuantities(postOutgoing.Shipments), Message = "Out Of Stock" });

            return Ok(outgoing);
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
                    return BadRequest(new BadRequestErrorModel() { Code = (int)OutgoingBadErrorCodes.DUPLICATE, Message = e.Message });
                else
                    throw;
            }
        }
        public IActionResult GetProductListByOrderId([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            return Ok(_outgoingSupervisor.GetProductListByOrderId(Id));
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
                if (e is OutgoingShipmentNotOperableException)
                    return BadRequest(new BadRequestErrorModel() { Message = e.Message, Code = (int)OutgoingBadErrorCodes.OUTGOINGSHIPMENT_NOT_OPERABLE });
                if (e is DuplicateNameException)
                    return BadRequest(new BadRequestErrorModel() { Message = e.Message, Code = (int)OutgoingBadErrorCodes.DUPLICATE_SHIPMENT });
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