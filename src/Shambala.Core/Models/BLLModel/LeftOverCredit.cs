

namespace Shambala.Core.Models.BLLModel
{

    public class LeftOverCreditBLL
    {
           public decimal Credit{get;set;}
           public short ShopId{get;set;}
           public int OutgoingShipmentId{get;set;}
           public bool IsCleared{get;set;}
    }
}
