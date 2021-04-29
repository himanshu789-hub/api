using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.DTOModels;
using System.Collections.Generic;
namespace Shambala.Controllers

{
    public class ProductController : ControllerBase
    {
        IProductSupervisor _productSupervisor;
        public ProductController(IProductSupervisor supervisor)
        {
            _productSupervisor = supervisor;
        }
        public async Task<IActionResult> Add(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Root.Errors);

            bool IsAdded = await _productSupervisor.AddAsync(incomingShipmentDTOs);
            if (IsAdded)
                return Ok();
            else
                return BadRequest();
        }
        public ActionResult GetAllWithoutLismit()
        {
            return new OkObjectResult(_productSupervisor.GetAll());
        }

    }
}