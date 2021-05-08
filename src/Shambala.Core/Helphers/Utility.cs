
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
            foreach (var Invoice in invoices)
            {
                foreach (SoldItemsDTO item in Invoice.SoldItems)
                {

                    Invoice NewInvoice = new Invoice();
                    NewInvoice.CaretSize = Invoice.CaretSize;
                    NewInvoice.OutgoingShipmentIdFk = Invoice.OutgoingShipmentId;
                    NewInvoice.SchemeIdFk = Invoice.SchemeId;
                    NewInvoice.ShopIdFk = Invoice.ShopId;
                    NewInvoice.ProductIdFk = item.ProductId;
                    NewInvoice.FlavourIdFk = (byte)item.FlavourId;
                    NewInvoice.QuantityPurchase = (short)item.Quantity;
                    Result.Add(NewInvoice);
                }

            }
            return Result;

        }
    }
}