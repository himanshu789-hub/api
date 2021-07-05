using System;
using System.Collections.Generic;
using Shambala.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Shambala.Repository
{
    using Domain;
    using Core.Models.BLLModel;
    using Helpher;
    static class QuerableMethods
    {
        public static IEnumerable<InvoiceAggreagateDetailBLL> GetAggreatesQueryableByShopId(ShambalaContext context, Expression<Func<Invoice, bool>> expr1, short[] shopIds)
        {
            if (shopIds == null)
                throw new System.ArgumentNullException();

            Expression<Func<Invoice, bool>> combine = null;
            foreach (var shopId in shopIds)
                combine = combine.OrElse<Invoice>(e => e.ShopIdFk == shopId);
            if (expr1 != null)
                combine.AndAlso<Invoice>(expr1);

            return context.Invoice.Where(combine)
            .GroupJoin(context.Debit.GroupBy(e => new { e.OutgoingShipmentIdFk, e.ShopIdFk }).Select(e => new { ClearedPrice = e.Sum(s => s.Amount), e.Key.OutgoingShipmentIdFk, e.Key.ShopIdFk }),
                          k => new { k.OutgoingShipmentIdFk, k.ShopIdFk }, l => new { l.OutgoingShipmentIdFk, l.ShopIdFk },
                        (k, l) => new { Invoice = k, Credit = l })
                        .SelectMany(x => x.Credit.DefaultIfEmpty(), (x, y) => new InvoiceAggreagateDetailBLL
                        {
                            Id = x.Invoice.Id,
                            DateCreated = x.Invoice.DateCreated,
                            TotalPrice = x.Invoice.Price,
                            ShopId = x.Invoice.ShopIdFk,
                            IsCleared = x.Invoice.IsCleared,
                            OutgoingShipmentId = x.Invoice.OutgoingShipmentIdFk,
                            TotalDueCleared = y.ClearedPrice
                        });
        }
    }
}