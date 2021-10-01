

using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace Shambala.Core.Helphers
{
    using Domain;
    using Models.DTOModel;
    using Models;
    public class AfterMapper
    {
        public static System.Action<OutgoingShipmentDetailTransferDTO, Product> setSchemePerCaret = (shipmentDetail, p) =>
        {
            shipmentDetail.SchemeInfo.SchemeQuantity = Utility.GetSchemeQuantityPerCaret(shipmentDetail.TotalQuantityShiped, shipmentDetail.SchemeInfo.TotalQuantity, p.CaretSize);
            foreach (CustomCaratPriceDTO customCaratPrice in shipmentDetail.CustomCaratPrices.Prices)
            {
                Product newProduct = new Product
                {
                    CaretSize = p.CaretSize,
                    Id = p.Id,
                    Name = p.Name,
                    PricePerCaret = customCaratPrice.PricePerCarat,
                    SchemeQuantity = p.SchemeQuantity
                };
                shipmentDetail.CustomCaratPrices.TotalPrice += Utility.GetTotalProductPrice(newProduct, customCaratPrice.Quantity);
            }
        };
        public static IEnumerable<OutgoingShipmentDetailTransferDTO> OutgoingShipmentTransferDTODetails(IEnumerable<OutgoingShipmentDetailTransferDTO> outgoingShipmentDetails, IEnumerable<Product> products)
        {
            foreach (var detail in outgoingShipmentDetails)
            {
                AfterMapper.setSchemePerCaret(detail, products.First(e => e.Id == detail.ProductId));
            }
            return outgoingShipmentDetails;
        }
        public static IEnumerable<OutgoingShipmentAggegateDetailDTO> OutgoingAggregateDetails(IEnumerable<OutgoingShipmentAggegateDetailDTO> aggegateDetailDTOs, IEnumerable<Product> products)
        {
            foreach (OutgoingShipmentAggegateDetailDTO detail in aggegateDetailDTOs)
            {
                Product product = products.First(e => e.Id == detail.ProductId);
                setSchemePerCaret(detail, product);
                detail.CaretSize = product.CaretSize;
                detail.UnitPrice = product.PricePerCaret;
            }
            return aggegateDetailDTOs;
        }
    }
    class Utility
    {
        static public bool IsDueCompleted(decimal DuePrice)
        {
            return DuePrice == 0;
        }
        static public IEnumerable<ShopCreditOrDebitDTO> CheckDebitUnderGivenBalance(IEnumerable<ShopCreditOrDebitDTO> recieveBalances, IEnumerable<InvoiceAggreagateDetailBLL> leftOverCreditBLLs)
        {
            IList<ShopCreditOrDebitDTO> leftOverCredit = new List<ShopCreditOrDebitDTO>();
            foreach (var receieveDebt in recieveBalances)
            {
                decimal totalCreditAmount = leftOverCreditBLLs.Where(e => e.ShopId == receieveDebt.ShopId && !e.IsCleared).Sum(e => e.TotalPrice - e.TotalDueCleared);
                if (totalCreditAmount < receieveDebt.Amount)
                {
                    leftOverCredit.Add(new ShopCreditOrDebitDTO() { Amount = totalCreditAmount, ShopId = receieveDebt.ShopId });
                }
            }
            return leftOverCredit;
        }
        static public byte GetSchemeQuantityPerCaret(int totalQuantityOfProduct, short totalSchemeQuantity, short caretSize)
        {
            int totalCaret = decimal.ToInt16(System.Math.Floor((decimal)totalQuantityOfProduct / caretSize));
            if (totalCaret == 0)
                return 0;
            return ((byte)decimal.ToInt32(totalSchemeQuantity / totalCaret));
        }
        static public decimal GetTotalProductPrice(Product Product, short quantity)
        {
            short[] cp = GetTotalCaratandPiece(Product.CaretSize, quantity);
            return Product.PricePerCaret * cp[0] + cp[1] * Utility.CalculatePricePerBottleOfProduct(Product);
        }
        static public short[] GetTotalCaratandPiece(short CaretSize, short quantity)
        {
            short carat = ((short)System.Math.Floor(decimal.Divide(quantity, CaretSize)));
            short piece = (short)(quantity % CaretSize);
            return new short[] { carat, piece };
        }
        static public short GetTotalSchemeQuantity(int totalProductQuantity, short caretSize, byte schemequantity)
        {
            return (short)(System.Math.Floor((decimal)(totalProductQuantity / caretSize)) * schemequantity);
        }
        static public IEnumerable<ShipmentDTO> GetReturnShipmentInfoList(IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails)
        {
            return outgoingShipmentDetails.Select(e => new ShipmentDTO()
            {
                FlavourId = e.FlavourIdFk,
                ProductId = e.ProductIdFk,
                TotalRecievedPieces = e.TotalQuantityReturned
            });
        }
        static public decimal CalculatePricePerBottleOfProduct(Product product) => decimal.Round(product.PricePerCaret / product.CaretSize, 2);
    }
}