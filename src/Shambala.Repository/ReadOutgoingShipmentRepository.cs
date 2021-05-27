using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Shambala.Repository
{
    using Core.Contracts.Repositories;
    using Infrastructure;

    using Shambala.Core.Models.DTOModel;
    using Shambala.Domain;

    public class ReadOutgoingShipmentRepository : IReadOutgoingShipmentRepository
    {
        readonly ShambalaContext context;
        public ReadOutgoingShipmentRepository(ShambalaContext context)
        {
            this.context = context;
        }

        public IEnumerable<OutgoingShipment> GetShipmentsBySalesmnaIdAndDate(short salesmanId, DateTime date)
        {
            var DateCreated = date.ToUniversalTime().Date;
            return context.OutgoingShipment.Include(e => e.SalesmanIdFkNavigation).Where(e => e.SalesmanIdFk == salesmanId && e.DateCreated.Date == DateCreated).ToList();
        }

        public IEnumerable<OutgoingShipmentProductInfoDTO> GetProductsById(int orderId)
        {

            IEnumerable<OutgoingShipmentProductInfoDTO> OutgoingShipmentDettailInfos = context.OutgoingShipmentDetails
            .AsNoTracking()
            .Include(e => e.ProductIdFkNavigation)
            .Include(e => e.FlavourIdFkNavigation)
            .Where(e => e.OutgoingShipmentIdFk == orderId)
            .Select(e => new OutgoingShipmentProductInfoDTO
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

    }
}