
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
    [System.Serializable]
    public class ShipmentNotVaidException : System.Exception
    {
        public ShipmentNotVaidException():base("Product In Shipment Not Exists") { }
        public ShipmentNotVaidException(string message) : base(message) { }
        public ShipmentNotVaidException(string message, System.Exception inner) : base(message, inner) { }
        protected ShipmentNotVaidException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class ShipmentReturnQuantityExceedException : System.Exception
    {
        public ShipmentReturnQuantityExceedException():base("Shipment Return Quantity Exceeded") { }
        public ShipmentReturnQuantityExceedException(string message) : base(message) { }
        public ShipmentReturnQuantityExceedException(string message, System.Exception inner) : base(message, inner) { }
        protected ShipmentReturnQuantityExceedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    [System.Serializable]
    public class SchemeProductQuantityExceedException : System.Exception
    {
        public SchemeProductQuantityExceedException():base("Scheme Product Quantity Exceed") { }
        public SchemeProductQuantityExceedException(string message) : base(message) { }
        public SchemeProductQuantityExceedException(string message, System.Exception inner) : base(message, inner) { }
        protected SchemeProductQuantityExceedException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}