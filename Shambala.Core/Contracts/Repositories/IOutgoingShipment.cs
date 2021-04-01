using Shambala.Domain;
using System.Collections.Generic;

namespace Shambala.Core.Contracts.Repositories
{
   public interface IOutgoingShipmentRepository
   {
       bool Add(OutgoingShipment outgoingShipment,IEnumerable<OutgoingShipmentDetail> OutgoingShipmentDetails);
       bool ChangeStatus(string status);
   }    
}