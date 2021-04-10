using System.Collections.Generic;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Supervisors
{
    public interface IProductSupervisor
    {
        IEnumerable<ProductDTO> GetAll();
    }
}