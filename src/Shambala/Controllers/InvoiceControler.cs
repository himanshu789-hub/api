using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
namespace Shambala.Controllers
{
    using Core.Contracts.Supervisors;
    using Core.Helphers;
    public class InvoiceController : ControllerBase
    {
        readonly IInvoiceSupervisor invoiceSupervisor;
        public InvoiceController(IInvoiceSupervisor supervisor)
        {
            invoiceSupervisor = supervisor;
        }

        [HttpGet]
        public IActionResult GetInvoiceDetail([FromQuery][BindRequired] short shopId, [FromQuery] System.DateTime? date, [FromQuery] int? page, [FromQuery] InvoiceStatus? status)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            return Ok(invoiceSupervisor.GetInvoiceDetailByShopId(shopId, date, status, page.HasValue ? page.Value : 1));
        }
        public IActionResult GetInvoiceWithCreditLog([FromQuery][BindRequired] short shopId, [FromQuery][BindRequired] int shipmentId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
            return Ok(invoiceSupervisor.GetShopInvoiceWithCreditLog(shipmentId, shopId));
        }
        // public IActionResult GetInvoiceBill([FromQuery][BindRequired] short shopId, [FromQuery][BindRequired] int shipmentId)
        // {
        //     if (!ModelState.IsValid)
        //         return BadRequest(ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));
        //     return Ok(invoiceSupervisor.GetInvoiceBill(shipmentId, shopId));
        // }
    }
}