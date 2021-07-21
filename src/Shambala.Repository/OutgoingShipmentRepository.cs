using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using Shambala.Core.Models.BLLModel;
using Shambala.Core.Models.DTOModel;

namespace Shambala.Repository
{
    public class OutgoingShipmentRepository : Shambala.Repository.GenericLoading<OutgoingShipment>, IOutgoingShipmentRepository
    {
        ShambalaContext _context;
        public OutgoingShipmentRepository(ShambalaContext context) : base(context) => _context = context;
        public OutgoingShipment Add(OutgoingShipment outgoingShipment)
        {
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);
            var Entity = _context.OutgoingShipment.Add(outgoingShipment);
            return Entity.Entity;
        }

        public OutgoingShipment GetByIdWithNoTracking(int Id)
        {
            return _context.OutgoingShipment
                .Include(e => e.SalesmanIdFkNavigation)
                .Include(e => e.OutgoingShipmentDetails)
                .AsNoTracking()
                .First(e => e.Id == Id);
        }


        public bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedValue)
        {
            return _context.OutgoingShipment.AsNoTracking().First(e => e.Id == Id).Status == System.Enum.GetName(typeof(OutgoingShipmentStatus), expectedValue);
        }
        public bool Return(int outgoingShipmentId, IEnumerable<OutgoingShipmentDetails> returnShipments)
        {
            string name = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);

            OutgoingShipment outgoing = this.GetByIdWithNoTracking(outgoingShipmentId);

            foreach (var item in returnShipments)
            {
                OutgoingShipmentDetails ShipmentDetail = outgoing.OutgoingShipmentDetails
                .First(e => e.FlavourIdFk == item.FlavourIdFk && e.ProductIdFk == item.ProductIdFk);
                short ReturnQuantity = item.TotalQuantityShiped;
                short DefectedQuantity = item.TotalQuantityRejected;
                ShipmentDetail.TotalQuantityShiped -= (short)(ReturnQuantity - DefectedQuantity);
                ShipmentDetail.TotalQuantityRejected += (byte)DefectedQuantity;
                ShipmentDetail.TotalQuantityReturned = ReturnQuantity;
            }
            outgoing.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN);
            _context.Attach(outgoing).State = EntityState.Modified;
            return true;
        }

        public bool Complete(int Id)
        {
            ICollection<ProductReturnBLL> productReturnBLLs = new List<ProductReturnBLL>();
            OutgoingShipment outgoingShipment = _context.OutgoingShipment.FirstOrDefault(e => e.Id == Id);

            if (outgoingShipment == null || outgoingShipment.Status != System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN))
                return false;
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.COMPLETED);
            return true;
        }

        public OutgoingShipment GetAllDetailById(int Id)
        {
            return _context.OutgoingShipment.Include(e => e.SalesmanIdFkNavigation)
            .Include(e => e.OutgoingShipmentDetails)
            .ThenInclude(e=>e.ProductIdFkNavigation)
            .Include(e=>e.OutgoingShipmentDetails)
            .ThenInclude(e=>e.FlavourIdFkNavigation)
            .Include(e=>e.OutgoingShipmentDetails)
            .Include(e => e.CustomCaratPrice)
            .First(e => e.Id == Id);
        }
    }
}
