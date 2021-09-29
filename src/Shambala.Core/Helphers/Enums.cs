

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
        DUPLICATE = 1221, SCHEME_EXCEED, OUT_OF_STOCK, SCHEME_QUANTITY_NOT_VALID,SHIPED_QUANTITY_NOT_VALID,CUSTOM_CARAT_PRICE_NOT_VALID,SCHME_PRICE_NOT_VALID
    }
    public enum AggreagateStatus
    {
        INVALID_SCHEME_PRICE=1331,INVALID_SHIPED_PRICE,INVALID_NET_PRICE,INVALID_CUSTOM_CARAT_PRICE,INVALID_CUSTOM_CARAT_QUANTITY
    }
}