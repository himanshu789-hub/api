using Shambala.Domain;
using Shambala.Core.DTOModels;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using Shambala.Core.Exception;
namespace Shambala.Repository
{

    public class OutgoingShipmentRepository : Shambala.Repository.GenericLoading<OutgoingShipment>, IOutgoingShipmentRepository
    {
        ShambalaContext _context;
        public OutgoingShipmentRepository(ShambalaContext context) : base(context) => _context = context;
        public OutgoingShipment Add(OutgoingShipment outgoingShipment)
        {
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN);
            var Entity = _context.OutgoingShipment.Add(outgoingShipment);

            return Entity.Entity;
        }



        public OutgoingShipment GetByIdWithNoTracking(int Id)
        {
            return _context.OutgoingShipment.AsNoTracking()
                .Include(e => e.OutgoingShipmentDetails)
                .Include(e => e.SalesmanIdFkNavigation)
                .First(e => e.Id == Id);
        }

        public IEnumerable<OutgoingShipmentDettailInfo> GetProductsById(int orderId)
        {
            IEnumerable<OutgoingShipmentDettailInfo> OutgoingShipmentDettailInfos = _context.OutgoingShipmentDetails.AsNoTracking()
            .Where(e => e.Id == orderId)
            .Select(e => new OutgoingShipmentDettailInfo
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
            })
            .ToList();

            return OutgoingShipmentDettailInfos;
        }
        
        public bool CheckStatus(int Id, OutgoingShipmentStatus expectedValue)
        {
            return _context.OutgoingShipment.First(e => e.Id == Id).Status == System.Enum.GetName(typeof(OutgoingShipmentStatus), expectedValue);
        }
        public bool Return(int outgoingShipmentId, IEnumerable<OutgoingShipmentDetail> returnShipments)
        {
            string name = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);

            OutgoingShipment outgoing = this.GetByIdWithNoTracking(outgoingShipmentId);
            _context.Entry(outgoing);
            foreach (var item in returnShipments)
            {
                OutgoingShipmentDetail ShipmentDetail = outgoing.OutgoingShipmentDetails.First(e => e.FlavourIdFk == item.FlavourIdFk && e.ProductIdFk == item.ProductIdFk);
                int ReturnQuantity = item.TotalQuantityShiped;
                int DefectedQuantity = item.TotalQuantityRejected;
                ShipmentDetail.TotalQuantityShiped += (ReturnQuantity - DefectedQuantity);
                ShipmentDetail.TotalQuantityRejected += DefectedQuantity;
            }
            outgoing.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN);
            return true;
        }
    }
}
