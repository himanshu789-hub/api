using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Shambala.Repository
{
    using Core.Contracts.Repositories;
    using Infrastructure;
    using Shambala.Core.Models;
    using Shambala.Domain;
    using Shambala.Core.Models.DTOModel;

    public class ReadOutgoingShipmentRepository : IReadOutgoingShipmentRepository
    {
        readonly ShambalaContext context;
        public ReadOutgoingShipmentRepository(ShambalaContext context)
        {
            this.context = context;
        }

        public OutgoingShipmentAggregateBLL GetDetails(int Id)
        {
            var outgoingShipment = context.OutgoingShipment.Include(e=>e.SalesmanIdFkNavigation).Include(e=>e.Ledger).Where(e => e.Id == Id);
            var outgoingShipmentDetails = context.OutgoingShipmentDetails.Include(e => e.CustomCaratPrices)
            .Where(e => e.OutgoingShipmentIdFk == Id)
            .Join(context.Product, (od) => od.ProductIdFk, (p) => p.Id, (od, p) => new OutgoingDetailBLL
            {
                CaretSize = p.CaretSize,
                CustomCaratPrices = od.CustomCaratPrices,
                FlavourIdFk = od.FlavourIdFk,
                Id = od.Id,
                NetPrice = od.NetPrice,
                ProductIdFk = od.ProductIdFk,
                OutgoingShipmentIdFk = od.OutgoingShipmentIdFk,
                SchemeTotalPrice = od.SchemeTotalPrice,
                SchemeTotalQuantity = od.SchemeTotalQuantity,
                TotalQuantityRejected = od.TotalQuantityRejected,
                TotalQuantityReturned = od.TotalQuantityReturned,
                TotalQuantityShiped = od.TotalQuantityShiped,
                TotalQuantityTaken = od.TotalQuantityTaken,
                TotalShipedPrice = od.TotalShipedPrice,
                UnitPrice = p.PricePerCaret
            });
            if (context.Database.CurrentTransaction != null)
            {
                IEnumerable<OutgoingDetailBLL> outgoingDetailBLL1s = outgoingShipmentDetails.ToList();
                OutgoingShipment outgoingShipment2 = outgoingShipment.First();
                return ProvideAggreageteBLL(outgoingDetailBLL1s, outgoingShipment2);
            }
            using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                IEnumerable<OutgoingDetailBLL> outgoingDetailBLLs = outgoingShipmentDetails.ToList();
                OutgoingShipment outgoingShipment1 = outgoingShipment.First();
                return ProvideAggreageteBLL(outgoingDetailBLLs, outgoingShipment1);
            }
        }
        OutgoingShipmentAggregateBLL ProvideAggreageteBLL(IEnumerable<OutgoingDetailBLL> detailBLLs, OutgoingShipment outgoingShipment)
        {
            return new OutgoingShipmentAggregateBLL
            {
                Id = outgoingShipment.Id,
                OutgoingShipmentDetails = new List<OutgoingDetailBLL>(detailBLLs),
                Salesman = outgoingShipment.SalesmanIdFkNavigation,
                Status = outgoingShipment.Status,
                Ledger=outgoingShipment.Ledger
            };
        }

        public IEnumerable<OutgoingShipment> GetShipmentsBySalesmanIdAndAfterDate(short salesmanId, DateTime date)
        {
            var dateCreated = date.ToUniversalTime().Date;
            var query = context.OutgoingShipment.Include(e => e.SalesmanIdFkNavigation).Where(e => e.SalesmanIdFk == salesmanId && e.DateCreated.Date == dateCreated);
            if (context.Database.CurrentTransaction != null)
                return query.ToList();
            using (var transacion = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                return query.ToList();
        }

        // public IEnumerable<OutgoingShipmentProductInfoDTO> GetProductsById(int orderId)
        // {

        //     IEnumerable<OutgoingShipmentProductInfoDTO> OutgoingShipmentDettailInfos = context.OutgoingShipmentDetails
        //     .AsNoTracking()
        //     .Include(e => e.ProductIdFkNavigation)
        //     .Include(e => e.FlavourIdFkNavigation)
        //     .Where(e => e.OutgoingShipmentIdFk == orderId)
        //     .Select(e => new OutgoingShipmentProductInfoDTO
        //     {
        //         Product = new ProductInfo()
        //         {
        //             Id = e.ProductIdFkNavigation.Id,
        //             Name = e.ProductIdFkNavigation.Name
        //         },
        //         Flavour = new FlavourDTO()
        //         {
        //             Id = e.FlavourIdFkNavigation.Id,
        //             Quantity = e.TotalQuantityShiped - e.TotalQuantityRejected,
        //             Title = e.FlavourIdFkNavigation.Title
        //         }
        //     })
        //     .ToList();

        //     return OutgoingShipmentDettailInfos;
        // }

    }
}