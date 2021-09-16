using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using Shambala.Core.Models;
using Shambala.Core.Models.DTOModel;
using System;

namespace Shambala.Repository
{
    public class OutgoingShipmentRepository : Shambala.Repository.GenericLoading<OutgoingShipment>, IOutgoingShipmentRepository
    {
        ShambalaContext _context;
        public OutgoingShipmentRepository(ShambalaContext context) : base(context) => _context = context;

        public OutgoingShipment Add(OutgoingShipment outgoingShipment)
        {
            outgoingShipment.Id = 0;
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);
            var Entity = _context.OutgoingShipment.Add(outgoingShipment);
            return Entity.Entity;

        }

        public OutgoingShipment GetByIdWithNoTracking(int Id)
        {
            return _context.OutgoingShipment
                .Include(e => e.SalesmanIdFkNavigation)
                .Include(e => e.OutgoingShipmentDetails).ThenInclude(e => e.CustomCaratPrices)
                .AsNoTracking()
                .First(e => e.Id == Id);
        }


        public bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedValue)
        {
            return _context.OutgoingShipment.AsNoTracking().First(e => e.Id == Id).Status == System.Enum.GetName(typeof(OutgoingShipmentStatus), expectedValue);
        }

        public IEnumerable<OutgoingShipment> GetBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            return _context.OutgoingShipment.AsNoTracking().Where(e => e.SalesmanIdFk == salesmanId && e.DateCreated.Date >= date.Date).ToList();
        }
    }
}
