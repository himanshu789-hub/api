using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;

namespace Shambala.Controllers
{
    public class IncomingShipmentController : ControllerBase
    {
        IIncomingShipmentSupervisor _incomingShipmentSupervisor;
        public IncomingShipmentController(IIncomingShipmentSupervisor incomingShipmentSupervisor)
        {
            _incomingShipmentSupervisor = incomingShipmentSupervisor;
        }
        

     }
}