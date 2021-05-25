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

        IEnumerable<InvoiceAggreateBLL> GetInvoiceAggreatesQuerableByShopId(Func<Invoice, bool> predicate, short shopId)
        {
            return _context.Invoice.Where(predicate)
                        .GroupBy(e => e.OutgoingShipmentIdFk)
                        .Select(e => new { e.Key, SoldPrice = e.Sum(s => s.SellingPrice), CostPrice = e.Sum(s => s.CostPrice) })
                        .GroupJoin(_context.Credit.Where(e => e.ShopIdFk == shopId).GroupBy(e => e.OutgoingShipmentIdFk)
                        .Select(e => new { ClearedPrice = e.Sum(s => s.Amount), e.Key }),
                          k => k.Key, l => l.Key,
                        (k, l) => new { Invoice = k, Credit = l })
                        .SelectMany(x => x.Credit.DefaultIfEmpty(), (x, y) => new InvoiceAggreateBLL
                        {
                            SoldPrice = x.Invoice.SoldPrice,
                            CostPrice = x.Invoice.CostPrice,
                            OutgoingShipmentId = x.Invoice.Key,
                            DuePrice = x.Invoice.SoldPrice - ((y?.ClearedPrice) ?? 0)
                        });
        }
        public InvoiceAggreateBLL GetInvoiceByShopAndShipmentId(short shopId, int shipmentId)
        {
            var query = this.GetInvoiceAggreatesQuerableByShopId((e) => e.ShopIdFk == shopId && e.OutgoingShipmentIdFk == shipmentId, shopId);
            query.First();
            throw new NotImplementedException();
        }
        public IEnumerable<InvoiceDetailBLL> GetInvoicesByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page, int count)
        {
            Func<Invoice, bool> predicate = e => e.ShopIdFk == shopId;
            if (date.HasValue)
            {
                var dateSearch = date.Value.Date;
                predicate = e => e.ShopIdFk == shopId && e.DateCreated.Date >= dateSearch;
            }

            var query = this.GetInvoiceAggreatesQuerableByShopId(predicate, shopId);

            if (status.HasValue)
            {
                if (status.Value == InvoiceStatus.COMPLETED)
                    query.Where(e => e.SoldPrice - e.DuePrice == 0);
                else
                    query.Where(e => e.SoldPrice - e.DuePrice > 0);
            };
            query = query.Skip((page - 1) * count).Take(count);

            var withSchemeQuery = query.Join(_context.Invoice.Include(e => e.SchemeIdFkNavigation).Where(e => e.ShopIdFk == shopId), e => e.OutgoingShipmentId, m => m.OutgoingShipmentIdFk, (e, m) =>
            new InvoiceDetailBLL()
            {
                TotalPrice = e.CostPrice,
                TotalDuePrice = e.DuePrice,
                Scheme = m.SchemeIdFkNavigation,
                TotalSellingPrice = e.SoldPrice,
                DateCreated = m.DateCreated,
                OutgoingShipmentId = m.OutgoingShipmentIdFk,
                ShopId = m.ShopIdFk
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