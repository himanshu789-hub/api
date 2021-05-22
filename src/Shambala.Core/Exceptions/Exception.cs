
namespace Shambala.Core.Exception
{
    using Helphers;
    
    [System.Serializable]
    public class DuplicateShipmentsException : System.Exception
    {
        public DuplicateShipmentsException() { }
        public DuplicateShipmentsException(string message) : base(message) { }
        public DuplicateShipmentsException(string message, System.Exception inner) : base(message, inner) { }
        protected DuplicateShipmentsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class OutgoingShipmentNotOperableException : System.Exception
    {
        public OutgoingShipmentNotOperableException(OutgoingShipmentStatus status) : base($"Expected Outgoing Shipment To Be {System.Enum.GetName(typeof(OutgoingShipmentStatus), (int)status)}")
        {

        }
        protected OutgoingShipmentNotOperableException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class QuantityOutOfStockException : System.Exception
    {
        public QuantityOutOfStockException() { }
        public QuantityOutOfStockException(string message) : base(message) { }
        public QuantityOutOfStockException(string message, System.Exception inner) : base(message, inner) { }
        protected QuantityOutOfStockException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class CreditFlorishException : System.Exception
    {
        public CreditFlorishException():base("Invalid Credit Amount") { }
        public CreditFlorishException(string message) : base(message) { }
        public CreditFlorishException(string message, System.Exception inner) : base(message, inner) { }
        protected CreditFlorishException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}