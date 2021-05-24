using Shambala.Core.Contracts.Repositories;
using Shambala.Core.Helphers;
using Shambala.Core.Models.DTOModel;
using Shambala.Domain;
using Shambala.Infrastructure;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Shambala.Repository
{
    using Core.Models.BLLModel;
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        ShambalaContext _context;
        public InvoiceRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }

        public decimal GetAggreate(int outgoingShipmentId, int shopId)
        {
            return _context.Invoice.Where(e => e.OutgoingShipmentIdFk == outgoingShipmentId && e.SchemeIdFk == shopId).Sum(e => e.SellingPrice);
        }

        public IEnumerable<InvoiceDetailBLL> GetInvoicesByShopId(int shopId, DateTime? date, InvoiceStatus? status, int page, int count)
        {
            Func<Invoice, bool> predicate = e => e.ShopIdFk == shopId;
            if (date.HasValue)
                predicate = e => e.ShopIdFk == shopId && e.DateCreated.Date >= date.Value.Date;

            var query = _context.Invoice.Where(predicate)
            .GroupBy(e => new { e.ShopIdFk, e.OutgoingShipmentIdFk })
            .Select(e => new { e.Key.OutgoingShipmentIdFk, e.Key.ShopIdFk, SoldPrice = e.Sum(s => s.SellingPrice), CostPrice = e.Sum(s => s.CostPrice) })
            .Join(_context.Credit
            .GroupBy(e => new { e.ShopIdFk, e.OutgoingShipmentIdFk })
            .Select(e => new { e.Key.OutgoingShipmentIdFk, e.Key.ShopIdFk, ClearedPrice = e.Sum(s => s.Amount) }),
              k => new { k.ShopIdFk, k.OutgoingShipmentIdFk }, l => new { l.ShopIdFk, l.OutgoingShipmentIdFk },
            (k, l) => new { k.ShopIdFk, k.OutgoingShipmentIdFk, k.CostPrice, k.SoldPrice, DuePrice = (k.SoldPrice - l.ClearedPrice) });

            if (status.HasValue)
            {
                if (status.Value == InvoiceStatus.COMPLETED)
                    query.Where(e => e.SoldPrice - e.DuePrice == 0);
                else
                    query.Where(e => e.SoldPrice - e.DuePrice > 0);
            };
            query = query.Skip(page - 1 * count).Take(count);

            var withSchemeQuery = query.Join(_context.Invoice.Include(e => e.SchemeIdFkNavigation), e => new { e.ShopIdFk, e.OutgoingShipmentIdFk }, m => new { m.ShopIdFk, m.OutgoingShipmentIdFk }, (e, m) =>
            new InvoiceDetailBLL()
            {
                CostPrice = e.CostPrice,
                DuePrice = e.DuePrice,
                Scheme = m.SchemeIdFkNavigation,
                SellingPrice = e.SoldPrice,
                DateCreated = m.DateCreated,
                OutgoingShipmentId = m.OutgoingShipmentIdFk,
                ShopId = e.ShopIdFk
            });

            if (_context.Database.CurrentTransaction == null)
            {
                using (var transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    return withSchemeQuery.ToList();
                }
            }
            return withSchemeQuery.ToList();

        }
    }
}