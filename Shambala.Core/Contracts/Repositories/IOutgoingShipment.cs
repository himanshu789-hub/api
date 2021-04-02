using Shambala.Domain;
using System.Collections.Generic;

namespace Shambala.Core.Contracts.Repositories
{
   public interface IOutgoingShipmentRepository
   {
       OutgoingShipment Add(OutgoingShipment outgoingShipment,IEnumerable<OutgoingShipmentDetail> OutgoingShipmentDetails);
       bool ChangeStatus(int Id,string status);
   }    
}