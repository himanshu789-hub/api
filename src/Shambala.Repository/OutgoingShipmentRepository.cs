using Shambala.Domain;
using Shambala.Core.DTOModels;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using System.Linq;
public class OutgoingShipmentRepository : IOutgoingShipmentRepository
{
    ShambalaContext _context;
    public OutgoingShipmentRepository(ShambalaContext context) => _context = context;
    public OutgoingShipment Add(OutgoingShipment outgoingShipment)
    {
        foreach (var item in outgoingShipment.OutgoingShipmentDetails)
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

    public IEnumerable<OutgoingShipmentDettailInfo> GetProductById(int orderId)
    {
        IEnumerable<OutgoingShipmentDettailInfo> OutgoingShipmentDettailInfos = _context.OutgoingShipmentDetails.Where(e => e.Id == orderId).Select(e => new OutgoingShipmentDettailInfo
        {
            Product = new ProductInfo()
            {
                Id = e.ProductIdFkNavigation.Id,
                Name = e.ProductIdFkNavigation.Name,
                CaretSize = e.CaretSize
            },
            Flavour = new FlavourDTO()
            {
                Id = e.FlavourIdFkNavigation.Id,
                Quantity = e.TotalQuantityShiped - e.TotalQuantityRejected,
                Title = e.FlavourIdFkNavigation.Title
            }
        }).ToList();
        return OutgoingShipmentDettailInfos;
    }

    
}