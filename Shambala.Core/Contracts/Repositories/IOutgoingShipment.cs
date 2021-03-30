

namespace Shambala.Core.Contracts.Repositories
{
   public interface IOutgoingShipment
   {
       boolean Add(OutgoingShipment outgoingShipment,IEnumerable<OutgoingShipmentDetails> OutgoingShipmentDetails);
       boolean ChangeStatus(string status);
   }    
}