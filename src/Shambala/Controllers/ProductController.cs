using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
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
        [HttpPost]
        public async Task<IActionResult> Add(IEnumerable<ShipmentDTO> incomingShipmentDTOs)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Root.Errors);

            bool IsAdded = await _productSupervisor.AddAsync(incomingShipmentDTOs);
            if (IsAdded)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAllWithoutLimit()
        {
            return Ok(_productSupervisor.GetAll());
        }
        
        [HttpGet]
        public IActionResult GetAllWithLimit()
        {
            throw new System.NotImplementedException();
        }
        [HttpGet]
        public IActionResult GetProductsWithStockAndDispatch([FromQuery][Required]int productId,[FromQuery] byte? flavourId)
        {
            return Ok(_productSupervisor.GetProductsByLeftQuantityAndDispatch(productId,flavourId));
        }

    }
}