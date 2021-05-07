using Shambala.Infrastructure;
using Shambala.Domain;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Shambala.Core.DTOModels;

namespace Shambala.Repository
{
    public class ProductRepository : IProductRepository
    {
        ShambalaContext _context;
        public ProductRepository(ShambalaContext context) => _context = context;

        public bool AddQuantity(int productId, int flavourId, int quantity)
        {
            int value =  _context.Database.ExecuteSqlRaw("UPDATE Product_Flavour_Quantity SET Quantity = Quantity + {2} WHERE Product_Id_FK = {0} AND Flavour_Id_FK = {1}", productId, flavourId, quantity);
    
            return value > 0;
        }


        public IEnumerable<Product> GetAll()
        {
            return _context.Product.AsNoTracking().Include(e => e.ProductFlavourQuantity).ToList();
        }

        public IEnumerable<ProductInfoDTO> GetProductsInStockWithDispatchQuantity()
        {
            // using (var con = _context.Database.GetDbConnection())
            // {
            //     using (var cmd = con.CreateCommand())
            //     {
            //         cmd.CommandText = "SELECT OSD.Id,OSD.Product_Id_FK AS 'ProductId',OSD.Flavour_Id_FK AS 'FalvourId'," +
            //         "(sum(OSD.Total_Quantity_Shiped)-sum(OSD.Total_Quantity_Rejected)) AS 'QuantityDispatch',pfq.Quantity AS 'QuantityInStock' FROM" +
            //         " shambala.outgoing_shipment_details AS OSD JOIN shambala.outgoing_shipment AS OUTS ON OSD.Outgoing_Shipment_Id_FK=OUTS.Id" +
            //         " JOIN shambala.product_flavour_quantity AS pfq ON pfq.Flavour_Id_FK=OSD.Flavour_Id_FK AND pfq.Product_Id_FK=OSD.Product_Id_FK WHERE" +
            //         " OUTS.Status='RETURN' GROUP BY OSD.Product_Id_FK,OSD.Flavour_Id_FK;";
            //         var reader = cmd.ExecuteReader();
            //         if (reader.HasRows)
            //         {
            //             while (reader.Read())
            //             {

            //             }
            //         }
            //     }
            // }
            var result = _context.OutgoingShipment.Where(e => e.Status == "PENDING")
              .Join(_context.OutgoingShipmentDetails,
               e => e.Id, i => i.OutgoingShipmentIdFk,
              (e, i) => new
              {
                  Quantity = i.TotalQuantityShiped - i.TotalQuantityRejected,
                  i.ProductIdFk,
                  i.FlavourIdFk
              })
              .GroupBy(e => new { e.ProductIdFk, e.FlavourIdFk }).Select(e => new
              {
                  e.Key.ProductIdFk,
                  e.Key.FlavourIdFk,
                  QuantityInProcrument = e.Sum(s => s.Quantity)
              })
              .Join(_context.ProductFlavourQuantity, e => new { e.ProductIdFk, e.FlavourIdFk }, k => new { k.ProductIdFk, k.FlavourIdFk }, (k, i) =>
              new
              {
                  FlavourId = k.FlavourIdFk,
                  ProductId = k.ProductIdFk,
                  QuantityInProcrument = k.QuantityInProcrument,
                  QuantityInStock = i.Quantity
              })
              .Join(_context.Product, e => e.ProductId, i => i.Id, (k, i) => new
              {
                  k.ProductId,
                  k.QuantityInProcrument,
                  k.QuantityInStock,
                  k.FlavourId,
                  i.Name
              }).Join(_context.Flavour, e => e.FlavourId, f => f.Id, (e, f) => new
              {
                  ProductName = e.Name,
                  FlavourName = f.Title,
                  e.QuantityInProcrument,
                  e.QuantityInStock,
                  e.ProductId,
                  e.FlavourId
              })
              .ToList();
            IEnumerable<ProductInfoDTO> ProductInfoDTOs = result.GroupJoin(result, e => e.ProductId, f => f.ProductId, (e, f) => new ProductInfoDTO()
            {
                Id = e.ProductId,
                Name = e.ProductName,
                FlavourInfos = f.Where(g => g.ProductId == e.ProductId)
                .Select(s => new FlavourInfoDTO() { Id = s.FlavourId, QuantityDispatch = s.QuantityInProcrument, QuantityInStock = s.QuantityInStock, Title = s.FlavourName })
            });
            return ProductInfoDTOs;
            //   .Join(_context.ProductFlavourQuantity,
            //    e => new { e.ProductIdFk, e.FlavourIdFk }, k => new { k.ProductIdFk, k.FlavourIdFk },
            //    (k, i) => new { k.ProductIdFk, k.FlavourIdFk, QuantityInStock = i.Quantity, QuantityInProcrument = k.Quantity })
            //    .ToList();

            //   .Join(_context.ProductFlavourQuantity,
            //   e => new { e.ProductIdFk, e.FlavourIdFk }, i => new { i.ProductIdFk, i.FlavourIdFk },
            //   (e, i) => new
            //   {
            //       e.FlavourIdFk,
            //       e.ProductIdFk,
            //       e.Quantity
            //   }).

        }
    }
}