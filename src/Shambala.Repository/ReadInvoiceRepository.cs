using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

using System.Linq.Expressions;
namespace Shambala.Repository
{
    using Core.Contracts.Repositories;
    using Infrastructure;
    using Shambala.Core.Helphers;
    using Shambala.Core.Models.BLLModel;
    using Shambala.Domain;
    using Helpher;
    public class ReadInvoiceRepository : IReadInvoiceRepository
    {
        readonly ShambalaContext context;
        public ReadInvoiceRepository(ShambalaContext context)
        {
            this.context = context;
        }

        public InvoiceAggreagateDetailBLL GetAggreate(int outgoingShipmentId, short shopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceAggreagateDetailBLL> GetAllInvoiceByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page, int? count)
        {
            throw new NotImplementedException();
        }

        public InvoiceDetailWithInfoBLL GetAllInvoiceDetailOfShopByShipmentId(short shopId, int shipmentId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceAggreagateDetailBLL> GetNotClearedAggregateByShopIds(short[] shopIds)
        {
            throw new NotImplementedException();
        }
        // public InvoiceAggreagateDetailBLL GetAggreate(int outgoingShipmentId, short shopId)
        // {
        //     return QuerableMethods.GetAggreatesQueryableByShopId(context, e => !e.IsCleared && e.ShopIdFk == shopId && e.OutgoingShipmentIdFk == outgoingShipmentId, new short[] { shopId }).First();
        // }
        // class DistinctSingleInvoiceShipment : IEqualityComparer<Invoice>
        // {
        //     public bool Equals(Invoice x, Invoice y)
        //     {
        //         return x.OutgoingShipmentIdFk == y.OutgoingShipmentIdFk;
        //     }

        //     public int GetHashCode(Invoice obj)
        //     {
        //         return obj.GetHashCode();
        //     }
        // }

        // public IEnumerable<InvoiceAggreagateDetailBLL> GetAllInvoiceByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page, int? count)
        // {
        //     Expression<Func<Invoice, bool>> expr = e => e.ShopIdFk == shopId;

        //     if (date.HasValue)
        //     {
        //         var dateSearch = date.Value.Date;
        //         expr = (e) => e.DateCreated.Date >= dateSearch;
        //     }

        //     var query = QuerableMethods.GetAggreatesQueryableByShopId(context, expr, new short[] { shopId }).AsQueryable();

        //     if (status.HasValue)
        //     {
        //         if (status.Value == InvoiceStatus.COMPLETED)
        //         {
        //             query.Where(e => e.IsCleared);
        //         }
        //         else
        //             query.Where(e => !e.IsCleared);
        //     };
        //     if (count != null)
        //         query = query.Skip((page - 1) * count.Value).Take(count.Value);

        //     var withSchemeQuery = query.Join(
        //         context.Invoice.FromSqlRaw("SELECT * FROM shambala.invoice WHERE Shop_Id_FK={0} GROUP BY Outgoing_Shipment_Id_FK", shopId)
        //         .Include(e => e.SchemeIdFkNavigation),
        //      e => e.OutgoingShipmentId, m => m.OutgoingShipmentIdFk, (e, m) =>
        //     new InvoiceAggreagateDetailBLL()
        //     {
        //         TotalDueCleared = e.TotalDueCleared,
        //         Scheme = m.SchemeIdFkNavigation,
        //         TotalPrice = e.TotalPrice,
        //         DateCreated = m.DateCreated,
        //         OutgoingShipmentId = m.OutgoingShipmentIdFk,
        //         ShopId = m.ShopIdFk,
        //         IsCleared = e.IsCleared
        //     });

        //     if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
        //     {
        //         using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //         {
        //             return withSchemeQuery.ToList();
        //         }
        //     }
        //     return withSchemeQuery.ToList();
        // }

        // public InvoiceDetailWithInfoBLL GetAllInvoiceDetailOfShopByShipmentId(short shopId, int shipmentId)
        // {
        //     var query = QuerableMethods.GetAggreatesQueryableByShopId(context, e => e.OutgoingShipmentIdFk == shipmentId && e.ShopIdFk == shopId, new short[] { shopId });
        //     var withProperties = query.Join(
        //         context.Invoice.Include(e => e.OutgoingShipmentIdFkNavigation).ThenInclude(e => e.SalesmanIdFkNavigation).Include(e => e.SchemeIdFkNavigation)
        //         .Include(e => e.ShopIdFkNavigation).Where(e => e.OutgoingShipmentIdFk == shipmentId && e.ShopIdFk == shopId).Take(1),
        //         e => e.OutgoingShipmentId, f => f.OutgoingShipmentIdFk, (e, f) => new InvoiceDetailWithInfoBLL()
        //         {
        //             DateCreated = f.DateCreated,
        //             OutgoingShipmentId = f.OutgoingShipmentIdFk,
        //             Scheme = f.SchemeIdFkNavigation,
        //             ShopId = e.ShopId,
        //             TotalDueCleared = e.TotalDueCleared,
        //             TotalPrice = e.TotalPrice,
        //             OutgoingShipment = f.OutgoingShipmentIdFkNavigation,
        //             Shop = f.ShopIdFkNavigation
        //         });

        //     if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
        //     {
        //         using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //         {
        //             return withProperties.First();
        //         }
        //     }
        //     return withProperties.First();
        // }


        // public IEnumerable<InvoiceAggreagateDetailBLL> GetNotClearedAggregateByShopIds(short[] shopIds)
        // {
        //     var query = QuerableMethods.GetAggreatesQueryableByShopId(context, (e) => !e.IsCleared, shopIds);
        //     if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
        //     {
        //         using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
        //         {
        //             return query.ToList();
        //         }
        //     }
        //     return query.ToList();
        // }
    }
}