using Microsoft.AspNetCore.Mvc;
using Shambala.Core.Contracts.Supervisors;
using Shambala.Core.DTOModels;

namespace Shambala.Controllers

{
    public class ProductController : ControllerBase
    {
        IProductSupervisor _incomingShipmentSupervisor;
        public ProductController(IIncomingShipmentSupervisor incomingShipmentSupervisor)
        {
            _incomingShipmentSupervisor = incomingShipmentSupervisor;
        }

    }
}