using System.Collections.Generic;

namespace Shambala.Core.Contracts.Repositories
{
    using Domain;
    using Models.DTOModel;
    using Models.BLLModel;
    using Helphers;
    public interface IReadOutgoingShipmentRepository
    {
        IEnumerable<OutgoingShipment> GetShipmentsBySalesmnaIdAndDate(short salesmanId, System.DateTime date);
        IEnumerable<OutgoingShipmentProductInfoDTO> GetProductsById(int orderId);
    }
    public interface IReadInvoiceRepository
    {
        InvoiceAggreagateDetailBLL GetAggreate(int outgoingShipmentId, short shopId);
        IEnumerable<InvoiceAggreagateDetailBLL> GetAllInvoiceByShopId(short shopId, System.DateTime? date, InvoiceStatus? status, int page, int count);
        InvoiceDetailWithInfoBLL GetSingleInvoiceAllDetailByShopIdAndShipmentId(short shopId, int shipmentId);
        IEnumerable<InvoiceBillingInfoBLL> GetBill(short shopId, int shipmentId);
    }
    public interface ICreditReadRepository
    {
        IEnumerable<Credit> GetCreditLogs(short shopId,int shipmentId);
    }
}