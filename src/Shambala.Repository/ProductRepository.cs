using Shambala.Infrastructure;
using Shambala.Domain;
using Shambala.Core.Contracts.Repositories;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Shambala.Core.Models.DTOModel;
namespace Shambala.Repository
{

    class ProductInfoEquality : IEqualityComparer<ProductInfoDTO>
    {
        public bool Equals(ProductInfoDTO x, ProductInfoDTO y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(ProductInfoDTO obj)
        {
            return obj.GetHashCode();
        }
    }
    public class ProductRepository : IProductRepository
    {
        ShambalaContext _context;
        public ProductRepository(ShambalaContext context) => _context = context;

        public bool AddQuantity(int productId, int flavourId, int quantity)
        {
            int value = _context.Database.ExecuteSqlRaw("UPDATE Product_Flavour_Quantity SET Quantity = Quantity + {2} WHERE Product_Id_FK = {0} AND Flavour_Id_FK = {1}", productId, flavourId, quantity);

            return value > 0;
        }

        public bool DeductQuantityOfProductFlavour(int productId, int flavourId, int quantity)
        {
            int value = _context.Database.ExecuteSqlRaw("UPDATE Product_Flavour_Quantity SET Quantity = Quantity - {2} WHERE Product_Id_FK = {0} AND Flavour_Id_FK = {1}", productId, flavourId, quantity);
            return value > 0;
        }

        public IEnumerable<Product> GetAllWithNoTracking(DateTime? beforeDate = null)
        {
            var query = _context.Product.Include(e => e.ProductFlavourQuantity).ThenInclude(e => e.FlavourIdFkNavigation).AsQueryable();

            if (beforeDate.HasValue)
            {
                query = query.GroupJoin(_context.Scheme.Where(e => e.DateCreated <= beforeDate.Value).GroupBy(e => e.ProductIdFk)
                .Select(e => e.Max(s => s.Id)).Join(_context.Scheme, m => m, n => n.Id, (m, n) => n),
                 m => m.Id, n => n.ProductIdFk, (m, n) => new { Product = m, Schemes = n })
                 .SelectMany(xy => xy.Schemes.DefaultIfEmpty(), (x, y) => new { Product = x.Product, Scheme = y })
                 .Select(e => new Product()
                 {
                     CaretSize = e.Product.CaretSize,
                     Id = e.Product.Id,
                     Name = e.Product.Name,
                     PricePerCaret = e.Product.PricePerCaret,
                     ProductFlavourQuantity = e.Product.ProductFlavourQuantity,
                     SchemeQuantity = e.Scheme.Quantity
                 });
            }
            return query.AsNoTracking().ToList();
        }

        public ProductInfoDTO GetProductsInStockWithDispatchQuantity(int ProductId, byte? FlavourId)
        {
            System.Func<ProductFlavourQuantity, bool> expression;
            if (FlavourId.HasValue)
                expression = (e) => e.ProductIdFk == ProductId && e.FlavourIdFk == FlavourId;
            else
                expression = (e) => e.ProductIdFk == ProductId;

            var dispatch = _context.OutgoingShipment
             .Where(e => e.Status == "PENDING")
               .Join(_context.OutgoingShipmentDetails,
                e => e.Id, i => i.OutgoingShipmentIdFk,
               (e, i) => new
               {
                   Quantity = i.TotalQuantityShiped - i.TotalQuantityRejected,
                   i.ProductIdFk,
                   i.FlavourIdFk
               })
               .GroupBy(e => new { e.ProductIdFk, e.FlavourIdFk })
               .Select(e => new
               {
                   e.Key.ProductIdFk,
                   e.Key.FlavourIdFk,
                   QuantityInProcrument = e.Sum(s => s.Quantity)
               }).ToList();

            var list = _context.ProductFlavourQuantity.Where(expression).ToList()
              .GroupJoin(dispatch, e => new { e.ProductIdFk, e.FlavourIdFk }, f => new { f.ProductIdFk, f.FlavourIdFk }, (e, f) => new { e, f })
              .SelectMany(e => e.f.DefaultIfEmpty(), (k, f) => new { e = new { k.e.ProductIdFk, k.e.FlavourIdFk, k.e.Quantity }, f }).ToList();

            var result = list.Join(_context.Product, e => e.e.ProductIdFk, i => i.Id, (k, i) => new
            {
                ProductId = k.e.ProductIdFk,
                QuantityInStock = k.e.Quantity,
                QuantityInDispatch = k.f?.QuantityInProcrument,
                FlavourId = k.e.FlavourIdFk,
                i.Name,
                i.CaretSize
            }).Join(_context.Flavour, e => e.FlavourId, f => f.Id, (e, f) => new
            {
                ProductName = e.Name,
                FlavourName = f.Title,
                e.QuantityInDispatch,
                e.QuantityInStock,
                Id = e.ProductId,
                e.FlavourId,
                e.CaretSize
            })
            .ToList();
            IEnumerable<ProductInfoDTO> ProductInfoDTOs = result.GroupBy(e => e.Id).Select(e => e.First()).GroupJoin(result, e => e.Id, f => f.Id,
            (e, f) => new ProductInfoDTO()
            {
                Id = e.Id,
                CaretSize = e.CaretSize,
                Name = e.ProductName,
                FlavourInfos = f.Where(g => g.Id == e.Id)
                   .Select(s => new FlavourInfoDTO() { Id = s.FlavourId, QuantityInDispatch = s.QuantityInDispatch ?? 0, QuantityInStock = s.QuantityInStock, Title = s.FlavourName })
            }).ToList();

            return ProductInfoDTOs.First();
        }
        // public bool ReturnQuantity(IEnumerable<ProductReturnBLL> productReturnBLLs)
        // {
        //     foreach (var item in productReturnBLLs)
        //     {
        //         ProductFlavourQuantity productFlavourQuantity = _context.ProductFlavourQuantity.FirstOrDefault(e => e.ProductIdFk == item.ProductId && e.FlavourIdFk == item.FlavourId);
        //         if (productFlavourQuantity == null)
        //             return false;
        //         productFlavourQuantity.Quantity += (short)item.Quantity;
        //     }
        //     return true;
        // }
    }
}