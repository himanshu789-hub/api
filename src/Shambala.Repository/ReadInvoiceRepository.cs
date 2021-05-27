using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Shambala.Repository
{
    using Core.Contracts.Repositories;
    using Infrastructure;
    using Shambala.Core.Helphers;
    using Shambala.Core.Models.BLLModel;
    using Shambala.Domain;
    public class ReadInvoiceRepository : IReadInvoiceRepository
    {
        readonly ShambalaContext context;
        public ReadInvoiceRepository(ShambalaContext context)
        {
            this.context = context;
        }
        public InvoiceAggreagateBLL GetAggreate(int outgoingShipmentId, short shopId)
        {
            return QuerableMethods.GetAggreatesQueryableByShopId(context, e => e.ShopIdFk == shopId && e.OutgoingShipmentIdFk == outgoingShipmentId, shopId).First();
        }
        class DistinctSingleInvoiceShipment : IEqualityComparer<Invoice>
        {
            public bool Equals(Invoice x, Invoice y)
            {
                return x.OutgoingShipmentIdFk == y.OutgoingShipmentIdFk;
            }

            public int GetHashCode(Invoice obj)
            {
                return obj.GetHashCode();
            }
        }

        public IEnumerable<InvoiceAggreagateDetailBLL> GetAllInvoiceByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page, int count)
        {
            Func<Invoice, bool> predicate = e => e.ShopIdFk == shopId;
            if (date.HasValue)
            {
                var dateSearch = date.Value.Date;
                predicate = e => e.ShopIdFk == shopId && e.DateCreated.Date >= dateSearch;
            }

            var query = QuerableMethods.GetAggreatesQueryableByShopId(context, predicate, shopId);

            if (status.HasValue)
            {
                if (status.Value == InvoiceStatus.COMPLETED)
                    query.Where(e => e.TotalSellingPrice - e.TotalDuePrice == 0);
                else
                    query.Where(e => e.TotalSellingPrice - e.TotalDuePrice > 0);
            };
            query = query.Skip((page - 1) * count).Take(count);

            var withSchemeQuery = query.Join(
                context.Invoice.Include(e => e.SchemeIdFkNavigation).Where(e => e.ShopIdFk == shopId).Distinct(new DistinctSingleInvoiceShipment()),
             e => e.OutgoingShipmentId, m => m.OutgoingShipmentIdFk, (e, m) =>
            new InvoiceAggreagateDetailBLL()
            {

                TotalCostPrice = e.TotalPrice,
                TotalDuePrice = e.TotalDuePrice,
                Scheme = m.SchemeIdFkNavigation,
                TotalSellingPrice = e.TotalSellingPrice,
                DateCreated = m.DateCreated,
                OutgoingShipmentId = m.OutgoingShipmentIdFk,
                ShopId = m.ShopIdFk
            });

            if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
            {
                using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    return withSchemeQuery.ToList();
                }
            }
            return withSchemeQuery.ToList();
        }

        public InvoiceDetailWithInfoBLL GetSingleInvoiceAllDetailByShopIdAndShipmentId(short shopId, int shipmentId)
        {
            var query = QuerableMethods.GetAggreatesQueryableByShopId(context, e => e.OutgoingShipmentIdFk == shipmentId && e.ShopIdFk == shopId, shopId);
            var withProperties = query.Join(
                context.Invoice.Include(e => e.OutgoingShipmentIdFkNavigation).ThenInclude(e=>e.SalesmanIdFkNavigation).Include(e => e.SchemeIdFkNavigation)
                .Include(e => e.ShopIdFkNavigation).Where(e => e.OutgoingShipmentIdFk == shipmentId && e.ShopIdFk == shopId).Take(1),
                e => e.OutgoingShipmentId, f => f.OutgoingShipmentIdFk, (e, f) => new InvoiceDetailWithInfoBLL()
                {
                    DateCreated = f.DateCreated,
                    OutgoingShipmentId = f.OutgoingShipmentIdFk,
                    Scheme = f.SchemeIdFkNavigation,
                    ShopId = e.ShopId,
                    TotalDuePrice = e.TotalDuePrice,
                    TotalCostPrice = e.TotalPrice,
                    TotalSellingPrice = e.TotalSellingPrice,
                    OutgoingShipment = f.OutgoingShipmentIdFkNavigation,
                    Shop = f.ShopIdFkNavigation
                });

            if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
            {
                using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    return withProperties.First();
                }
            }
            return withProperties.First();
        }

        public IEnumerable<InvoiceBillingInfoBLL> GetBill(short shopId, int shipmentId)
        {
            var query = context.Invoice.Include(e => e.ProductIdFkNavigation).Include(e => e.FlavourIdFkNavigation).Where(e => e.ShopIdFk == shopId && e.OutgoingShipmentIdFk == shipmentId)
            .Select(e => new InvoiceBillingInfoBLL
            {
                CaretSize = e.CaretSize,
                DateCreated = e.DateCreated,
                CostPrice = e.CostPrice,
                FlavourName = e.FlavourIdFkNavigation.Title,
                Gstrate = e.Gstrate,
                ProductName = e.ProductIdFkNavigation.Name,
                QuantityDefected = e.QuantityDefected,
                QuantityPurchase = e.QuantityPurchase,
                SellingPrice = e.SellingPrice
            });
            if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
            {
                return query.ToList();
            }
            return query.ToList();
        }
    }
}