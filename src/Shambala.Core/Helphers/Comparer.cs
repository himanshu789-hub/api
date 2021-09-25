using Shambala.Core.Models.DTOModel;
using System.Collections.Generic;
using System;
class ShipmentComparer : IEqualityComparer<ShipmentDTO>
{
    public bool Equals(ShipmentDTO x, ShipmentDTO y)
    {
        return x.ProductId == y.ProductId && y.FlavourId == x.FlavourId;
    }

    public int GetHashCode(ShipmentDTO obj)
    {
        return Tuple.Create(obj.ProductId,obj.FlavourId).GetHashCode();
    }
}

class OutgoingShipmentDetailComparer : IEqualityComparer<OutgoingShipmentDetailBaseDTO>
{
    public bool Equals(OutgoingShipmentDetailBaseDTO x, OutgoingShipmentDetailBaseDTO y)
    {
        return x.OutgoingShipmentId==y.OutgoingShipmentId && x.ProductId==y.ProductId && x.FlavourId==y.FlavourId; 
    }

    public int GetHashCode(OutgoingShipmentDetailBaseDTO obj)
    {
        return Tuple.Create(obj.OutgoingShipmentId,obj.ProductId,obj.FlavourId).GetHashCode();
    }
}