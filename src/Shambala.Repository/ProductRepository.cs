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

        public bool AddQuantity(int productId, int flavourId, short quantity)
        {
            _context.Database.ExecuteSqlRaw("UPDATE Product_Flavour_Quantity SET Quantity = Quantity + {2} WHERE Product_Id_FK = {0} AND Flavour_ID_FK = {1}", productId, flavourId, quantity);
            return true;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Product.AsNoTracking().Include(e => e.ProductFlavourQuantity).ToList();
        }

        public IEnumerable<ProductInfoDTO> GetProductsInStockWithDispatchQuantity(int Id)
        {
            using (var con = _context.Database.GetDbConnection())
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT OSD.Id,OSD.Product_Id_FK AS 'ProductId',OSD.Flavour_Id_FK AS 'FalvourId'," +
                    "(sum(OSD.Total_Quantity_Shiped)-sum(OSD.Total_Quantity_Rejected)) AS 'QuantityDispatch',pfq.Quantity AS 'QuantityInStock' FROM" +
                    " shambala.outgoing_shipment_details AS OSD JOIN shambala.outgoing_shipment AS OUTS ON OSD.Outgoing_Shipment_Id_FK=OUTS.Id" +
                    " JOIN shambala.product_flavour_quantity AS pfq ON pfq.Flavour_Id_FK=OSD.Flavour_Id_FK AND pfq.Product_Id_FK=OSD.Product_Id_FK WHERE" +
                    " OUTS.Status='RETURN' GROUP BY OSD.Product_Id_FK,OSD.Flavour_Id_FK;";
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                           
                        }
                    }
                }
            }
            throw new System.NotImplementedException();
        }
    }
}