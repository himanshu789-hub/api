using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;
using System.Linq;

namespace Shambala.Controllers
{

    public class GenericController<T> : ControllerBase where T : class
    {
        readonly IGenericSupervisor<T> _supervisor;
        public GenericController(IGenericSupervisor<T> supervisor)
        {
            _supervisor = supervisor;
        }
        [HttpPost]
        public IActionResult Add([FromBody] T dto)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                T AddedEntity = _supervisor.Add(dto);
                if (AddedEntity != null)
                {
                    return Ok(AddedEntity);
                }
                return Ok("Not Added");
            }
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }
        [HttpPut]
        public IActionResult Update([FromBody] T dto)
        {

            if (ModelState.IsValid)
            {
                bool IsUpdated = _supervisor.Update(dto);
                if (IsUpdated)
                {
                    return Ok();
                }
                return BadRequest("Not Updated");
            }
            return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
        }
        [HttpGet]
        public IActionResult GetById([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Root.Errors);
            return Ok(_supervisor.GetById(Id));
        }

    }
    [Controller]
    public class SchemeController : GenericController<SchemeDTO>
    {
        public SchemeController(ISchemeSupervisor supervisor) : base(supervisor)
        {

        }
    }
    public class ShopController : GenericController<ShopDTO>
    {
        readonly IShopSupervisor _supervisor;
        public ShopController(IShopSupervisor supervisor) : base(supervisor)
        {
            _supervisor = supervisor;
        }
        public IActionResult GetInvoices([FromRoute] int shopId)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(_supervisor.GetDetailWithInvoices(shopId));

        }
        [HttpGet]
        public IActionResult GetAllByName([FromQuery] string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors.SelectMany(e => e.ErrorMessage)));
            return Ok(_supervisor.GetAllByName(name));
        }
    }
    public class SalesmanController : GenericController<SalesmanDTO>
    {
        readonly ISalesmanSupervisor _supervisor;
        public SalesmanController(ISalesmanSupervisor salesmanSupervisor) : base(salesmanSupervisor)
        {
            _supervisor = salesmanSupervisor;
        }
        public IActionResult GetAll()
        {
            return Ok(_supervisor.GetAllActive());
        }
    }
}