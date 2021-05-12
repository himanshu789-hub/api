using System.Collections.Generic;
using Shambala.Core.Models.DTOModel;
using System.Threading.Tasks;
namespace Shambala.Core.Contracts.Supervisors
{
    public interface IProductSupervisor
    {
        IEnumerable<ProductDTO> GetAll();
        Task<bool> AddAsync(IEnumerable<ShipmentDTO> incomingShipmentDTOs);
        ProductInfoDTO GetProductsByLeftQuantityAndDispatch(int productId,byte? flavourId);
    }
}