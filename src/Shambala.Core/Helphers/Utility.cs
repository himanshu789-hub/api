

using System.Collections.Generic;
using AutoMapper;
using System.Linq;

namespace Shambala.Core.Helphers
{
    using Domain;
    using Models.DTOModel;
    using Models.BLLModel;
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
        static public short GetSchemeQuantityPerCaret(int totalQuantityOfProduct, short totalSchemeQuantity, short caretSize)
        {
            return decimal.ToInt16(totalSchemeQuantity / System.Math.Floor((decimal)totalQuantityOfProduct / caretSize));
        }
        static public decimal GetTotalSchemePrice(Product schemeProduct,short quantity)
        {
            return (schemeProduct.PricePerCaret/schemeProduct.CaretSize)*quantity;
        }
        static public short GetTotalSchemeQuantity(int totalProductQuantity,short caretSize,byte schemequantity)
        {
            return (short) ((totalProductQuantity/caretSize) * schemequantity);
        }
        static public IEnumerable<ShipmentDTO> GetReturnShipmentInfoList(IEnumerable<OutgoingShipmentDetails> outgoingShipmentDetails)
        {
            return outgoingShipmentDetails.Select(e => new ShipmentDTO(){
                CaretSize = e.CaretSize,
                FlavourId = e.FlavourIdFk,
                ProductId = e.ProductIdFk,
                TotalRecievedPieces = e.TotalQuantityReturned
            });
        }
    }
}