

namespace Shambala.Core.Helphers
{
    public static class OperationStatusFlags
    {
        public static class OutgoingShipmentStatus
        {
            public const string Pending = "PENDING";
            public const string Completed = "FILLED";
        }
    }
    public class SchemeProductOptions
    {

        public const string Option = "SchemeProductInfo";

        public int ProductId { get; set; }
        public short FlavourId { get; set; }
    }
}