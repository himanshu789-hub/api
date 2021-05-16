
using System.Collections.Generic;
using AutoMapper;
namespace Shambala.Core.Helphers
{
    using Domain;
    using Models.DTOModel;
    public class Utility
    {

        public static IEnumerable<Invoice> ToInvoices(IEnumerable<PostInvoiceDTO> invoices)
        {

            ICollection<Invoice> Result = new List<Invoice>();
            foreach (var PostInvoice in invoices)
            {
                foreach (SoldItemsDTO item in PostInvoice.Invoices)
                {

                    Invoice NewInvoice = new Invoice();
                    NewInvoice.CaretSize = PostInvoice.CaretSize;
                    NewInvoice.OutgoingShipmentIdFk = PostInvoice.OutgoingShipmentId;
                    NewInvoice.SchemeIdFk = PostInvoice.SchemeId;
                    NewInvoice.ShopIdFk = PostInvoice.ShopId;
                    
                    NewInvoice.ProductIdFk = item.ProductId;
                    NewInvoice.FlavourIdFk = item.FlavourId;
                    NewInvoice.QuantityPurchase = item.Quantity;
                    Result.Add(NewInvoice);
                }

            }
            return Result;

        }
    }
}