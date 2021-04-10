using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;

namespace Shambala.Controllers
{
    public class OutgoingShipmentController : ControllerBase
    {
      IOutg   _incomingShipmentSupervisor;
        public OutgoingShipmentController(IIncomingShipmentSupervisor incomingShipmentSupervisor)
        {
            _incomingShipmentSupervisor = incomingShipmentSupervisor;
        }

    }
}