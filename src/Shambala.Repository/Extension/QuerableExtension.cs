using System;
using System.Collections.Generic;
using Shambala.Infrastructure;
using System.Linq;
namespace Shambala.Repository
{
    using Domain;
    using Core.Models.BLLModel;
    static class QuerableMethods
    {
        public static IEnumerable<InvoiceAggreagateBLL> GetAggreatesQueryableByShopId(ShambalaContext context, Func<Invoice, bool> predicate, short shopId)
        {
            return context.Invoice.Where(predicate)
                        .GroupBy(e => e.OutgoingShipmentIdFk)
                        .Select(e => new { e.Key, SoldPrice = e.Sum(s => s.SellingPrice), CostPrice = e.Sum(s => s.CostPrice) })
                        .GroupJoin(context.Credit.Where(e => e.ShopIdFk == shopId).GroupBy(e => e.OutgoingShipmentIdFk)
                        .Select(e => new { ClearedPrice = e.Sum(s => s.Amount), e.Key }),
                          k => k.Key, l => l.Key,
                        (k, l) => new { Invoice = k, Credit = l })
                        .SelectMany(x => x.Credit.DefaultIfEmpty(), (x, y) => new InvoiceAggreagateBLL
                        {
                            TotalSellingPrice = x.Invoice.SoldPrice,
                            ShopId = shopId,
                            TotalPrice = x.Invoice.CostPrice,
                            OutgoingShipmentId = x.Invoice.Key,
                            TotalDuePrice = x.Invoice.SoldPrice - ((y?.ClearedPrice) ?? 0)
                        });
        }
    }
}