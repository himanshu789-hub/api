

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
    public enum ConcurrencyErrorCode
    {
        Concurrency_Error=1441
    }

    public enum OutgoingErroCode
    {
        DUPLICATE = 1221, SCHEME_EXCEED, OUT_OF_STOCK, SCHEME_QUANTITY_NOT_VALID,SHIPED_QUANTITY_NOT_VALID,CUSTOM_CARAT_PRICE_NOT_VALID,SCHME_PRICE_NOT_VALID,
        INVALID_CUSTOM_CARAT_QUANTITY,SHIPED_PRICE_NOT_VALID
    }
}