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
            .Include(e=>e.OutgoingShipmentDetails)
            .Include(e => e.OutgoingShipmentDetails).ThenInclude(e=>e.ProductIdFkNavigation)
            .Include(e=>e.OutgoingShipmentDetails).ThenInclude(e=>e.FlavourIdFkNavigation)
            .Include(e => e.CustomCaratPrice)
            .First(e => e.Id == Id);
        }
    }
}
