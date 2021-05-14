namespace Shambala.Models
{
    public enum OutgoingBadErrorCodes
    {
        DUPLICATE = 101, QUANTITIES_OUT_OF_STOCK,OUTGOING_OUT_OF_STOCK,OUTGOINGSHIPMENT_NOT_OPERABLE,DUPLICATE_SHIPMENT
    }

    public class BadRequestErrorModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Model { get; set; }
    }
}