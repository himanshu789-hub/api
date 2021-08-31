using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;
class ShipmentComparer : IEqualityComparer<ShipmentDTO>
{
    public bool Equals(ShipmentDTO x, ShipmentDTO y)
    {
        return x.ProductId == y.ProductId && y.FlavourId == x.FlavourId;
    }

    public int GetHashCode(ShipmentDTO obj)
    {
        throw new System.NotImplementedException();
    }
}

class OutgoingShipmentDetailComparer : IEqualityComparer<OutgoingShipmentDetailDTO>
{
    public bool Equals(OutgoingShipmentDetailDTO x, OutgoingShipmentDetailDTO y)
    {
        return x.OutgoingShipmentId==y.OutgoingShipmentId && x.ProductId==y.ProductId && x.FlavourId==y.FlavourId; 
    }

    public int GetHashCode(OutgoingShipmentDetailDTO obj)
    {
        throw new System.NotImplementedException();
    }
}