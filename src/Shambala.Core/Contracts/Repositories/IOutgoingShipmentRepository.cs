using Shambala.Domain;
using System.Collections.Generic;
using Shambala.Core.DTOModels;
namespace Shambala.Core.Contracts.Repositories
{
   public interface IOutgoingShipmentRepository
   {
       OutgoingShipment Add(OutgoingShipment outgoingShipment);
       bool ChangeStatus(int Id,string status);
       IEnumerable<OutgoingShipmentDettailInfo> GetProductsById(int orderId);
   }    
}