using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> AddAsync([FromBody]IEnumerable<ShipmentDTO> incomingShipmentDTOs)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.SelectMany(e=>e.Errors.Select(e=>e.ErrorMessage)));

            bool IsAdded = await _productSupervisor.AddAsync(incomingShipmentDTOs);
            if (IsAdded)
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery]System.DateTime? date)
        {
            return Ok(_productSupervisor.GetAll(date));
        }
        
        [HttpGet]
        public IActionResult GetProductByIdWithStockAndDispatch([FromRoute][Required]int Id)
        {
            if(!ModelState.IsValid)
            return BadRequest(ModelState.Values.SelectMany(e=>e.Errors.Select(e=>e.ErrorMessage)));
            return Ok(_productSupervisor.GetProductsByLeftQuantityAndDispatch(Id,null));
        }

    }
}