

namespace Shambala.Core.Helphers
{
    public enum SchemeType : byte
    {
        Percentage = 1,
        Caret,
        Bottle
    }
    public enum OutgoingShipmentStatus
    {
        PENDING, FILLED
    }
    public enum InvoiceStatus
    {
        DUE = 1, COMPLETED
    }

    public enum OutgoingErroCode
    {
        DUPLICATE = 1221, SCHEME_EXCEED, OUT_OF_STOCK, SCHEME_QUANTITY_NOT_VALID,SHIPED_QUANTITY_NOT_VALID,CUSTOM_CARAT_PRICE_NOT_VALID
    }
}