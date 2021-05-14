using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using Shambala.Core.Models.BLLModel;
using Shambala.Core.Models.DTOModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

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

        public IEnumerable<OutgoingShipmentDetailInfo> GetProductsById(int orderId)
        {
            IEnumerable<OutgoingShipmentDetailInfo> OutgoingShipmentDettailInfos = _context.OutgoingShipmentDetails
            .AsNoTracking()
            .Include(e => e.ProductIdFkNavigation)
            .Include(e => e.FlavourIdFkNavigation)
            .Where(e => e.OutgoingShipmentIdFk == orderId)
            .Select(e => new OutgoingShipmentDetailInfo
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

        public bool CheckStatusWithNoTracking(int Id, OutgoingShipmentStatus expectedValue)
        {
            return _context.OutgoingShipment.AsNoTracking().First(e => e.Id == Id).Status == System.Enum.GetName(typeof(OutgoingShipmentStatus), expectedValue);
        }
        public bool Return(int outgoingShipmentId, IEnumerable<OutgoingShipmentDetail> returnShipments)
        {
            string name = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.PENDING);

            OutgoingShipment outgoing = this.GetByIdWithNoTracking(outgoingShipmentId);

            foreach (var item in returnShipments)
            {
                OutgoingShipmentDetail ShipmentDetail = outgoing.OutgoingShipmentDetails
                .First(e => e.FlavourIdFk == item.FlavourIdFk && e.ProductIdFk == item.ProductIdFk);
                int ReturnQuantity = item.TotalQuantityShiped;
                int DefectedQuantity = item.TotalQuantityRejected;
                ShipmentDetail.TotalQuantityShiped += (ReturnQuantity - DefectedQuantity);
                ShipmentDetail.TotalQuantityRejected += DefectedQuantity;
            }
            outgoing.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN);
            _context.Attach(outgoing).State = EntityState.Modified;
            return true;
        }

        public bool Complete(int Id, IEnumerable<OutgoingQuantityRejectedBLL> outgoingQuantityRejectedBLLs)
        {
            ICollection<ProductReturnBLL> productReturnBLLs = new List<ProductReturnBLL>();
            OutgoingShipment outgoingShipment = _context.OutgoingShipment.FirstOrDefault(e => e.Id == Id);

            if (outgoingShipment == null || outgoingShipment.Status != System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.RETURN))
                return false;
            if (outgoingQuantityRejectedBLLs != null)
            {
                _context.Entry(outgoingShipment).Reference(e => e.OutgoingShipmentDetails).Load();
                foreach (OutgoingQuantityRejectedBLL item in outgoingQuantityRejectedBLLs)
                {
                    OutgoingShipmentDetail outgoing = outgoingShipment.OutgoingShipmentDetails.FirstOrDefault(e => e.Id == item.Id);
                    outgoing.TotalQuantityRejected += item.TotalQuantityRejected;
                    outgoing.TotalQuantityShiped -= item.TotalQuantityRejected;
                }
            }
            outgoingShipment.Status = System.Enum.GetName(typeof(OutgoingShipmentStatus), OutgoingShipmentStatus.COMPLETED);
            return true;
        }

        public IEnumerable<OutgoingShipment> GetShipmentsBySalesmnaIdAndDate(short salesmanId, DateTime date)
        {
            return _context.OutgoingShipment.Where(e => e.SalesmanIdFk == salesmanId && e.DateCreated == date).ToList();
        }
    }
}
