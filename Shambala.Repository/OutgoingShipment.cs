using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
public class OutgoingShipmentRepository : IOutgoingShipmentRepository
{
    ShambalaContext _context;
    OutgoingShipmentRepository(ShambalaContext context) => _context = context;
    public OutgoingShipment Add(OutgoingShipment outgoingShipment, IEnumerable<OutgoingShipmentDetail> outgoingShipmentDetails)
    {
        foreach (var item in outgoingShipmentDetails)
            outgoingShipment.OutgoingShipmentDetails.Add(item);
        var Entity = _context.OutgoingShipment.Add(outgoingShipment);
        return Entity.Entity;
    }

    public bool ChangeStatus(int Id, string status)
    {
        var OutgoingShipment = _context.OutgoingShipment.Where(e => e.Id == Id).FirstOrDefault();
        if (OutgoingShipment == null)
            return false;
        OutgoingShipment.Status = status;
        return true;
    }
}