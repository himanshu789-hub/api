using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
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
        public IActionResult Add(IEnumerable<IncomingShipmentDTO> incomingShipmentDTOs)
        {
            if (ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Root.Errors);

            _productSupervisor.Add(incomingShipmentDTOs);
            return new OkObjectResult(true);
        }

    }
}