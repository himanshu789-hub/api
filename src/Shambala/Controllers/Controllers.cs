using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.DTOModels;
using System.Collections.Generic;

namespace Shambala.Controllers
{
    [Microsoft.AspNetCore.Mvc.NonController]
    public class GenericController<T> : ControllerBase where T : class
    {
        readonly IGenericSupervisor<T> _supervisor;
        public GenericController(IGenericSupervisor<T> supervisor)
        {
            _supervisor = supervisor;
        }
        public IActionResult Add(T dto)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                T AddedEntity = _supervisor.Add(dto);
                if (AddedEntity != null)
                {
                    return Ok();
                }
                return BadRequest("Not Added");
            }
            return BadRequest(ModelState.Root.Errors);
        }
        public IActionResult Update(T dto)
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
            return BadRequest(ModelState.Root.Errors);
        }
        public IActionResult GetById(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Root.Errors);
            return Ok(_supervisor.GetById(Id));
        }
    }

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
    }
    public class SalesmanController : GenericController<SalesmanDTO>
    {
        readonly ISalesmanSupervisor _supeervisor;
        public SalesmanController(ISalesmanSupervisor salesmanSupervisor) : base(salesmanSupervisor)
        {
            _supeervisor = salesmanSupervisor;
        }
        public IActionResult GetAll()
        {
            return Ok(_supeervisor.GetAllActive());
        }
    }
}