using System.Collections.Generic;
namespace Shambala.Domain
{
    public class OutgoingShipmentComparer : IEqualityComparer<OutgoingShipmentDetails>
    {
        public bool Equals(OutgoingShipmentDetails x, OutgoingShipmentDetails y)
        {
            return x.OutgoingShipmentIdFk == y.OutgoingShipmentIdFk && x.ProductIdFk == y.ProductIdFk && x.FlavourIdFk == y.FlavourIdFk;
        }

        public int GetHashCode(OutgoingShipmentDetails obj)
        {
            return obj.Id.GetHashCode() ;
        }
    }
    public class IncomingShipmentComparer : IEqualityComparer<IncomingShipment>
    {
        public bool Equals(IncomingShipment x, IncomingShipment y)
        {
            return x.ProductIdFk==y.ProductIdFk && x.FlavourIdFk==y.FlavourIdFk;
        }

        public int GetHashCode(IncomingShipment obj)
        {
            return obj.Id;
        }
    }
}