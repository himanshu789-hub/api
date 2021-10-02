using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using System;

namespace Shambala.Repository
{
    using Domain;
    public class OutgoingShipmentRepository : Shambala.Repository.GenericLoading<OutgoingShipment>, IOutgoingShipmentRepository
    {
        ShambalaContext _context;
        public OutgoingShipmentRepository(ShambalaContext context) : base(context) => _context = context;

        public OutgoingShipment Add(OutgoingShipment outgoingShipment)
        {
            outgoingShipment.Id = 0;
            outgoingShipment.RowVersion = 0;
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);
            foreach (var details in outgoingShipment.OutgoingShipmentDetails)
            {
                details.Id = 0;
            }
            var Entity = _context.OutgoingShipment.Add(outgoingShipment);
            return Entity.Entity;

        }

        public OutgoingShipment GetByIdWithNoTracking(int Id)
        {
            OutgoingShipment shipment = _context.OutgoingShipment
                .Include(e => e.SalesmanIdFkNavigation)
                .Include(e => e.OutgoingShipmentDetails).ThenInclude(e => e.CustomCaratPrices)
                .AsNoTracking()
                .First(e => e.Id == Id);
                
                return shipment;
        }


        public bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedValue)
        {
            return _context.OutgoingShipment.AsNoTracking().First(e => e.Id == Id).Status == System.Enum.GetName(typeof(OutgoingShipmentStatus), expectedValue);
        }

        public IEnumerable<OutgoingShipment> GetBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            return _context.OutgoingShipment.Where(e => e.SalesmanIdFk == salesmanId && e.DateCreated.Date >= date.Date).AsNoTracking().ToList();
        }

        public bool Update(OutgoingShipment outgoingShipment)
        {
            short newRowVersin = (short)(outgoingShipment.RowVersion + 1);
            try
            {
                int changes = _context.Database.ExecuteSqlRaw("UPDATE outgoing_shipment SET RowVersion={0} , Status={1} WHERE Id={2} AND RowVersion={3}",
                 newRowVersin, System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.FILLED), outgoingShipment.Id, outgoingShipment.RowVersion);
                if (changes > 0)
                {
                    return true;
                }
                throw new DbUpdateConcurrencyException();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
