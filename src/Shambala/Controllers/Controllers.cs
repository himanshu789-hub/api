using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using System.Threading.Tasks;
using Shambala.Core.DTOModels;
using System.Collections.Generic;

namespace Shambala.Controllers
{
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
        
    }
}