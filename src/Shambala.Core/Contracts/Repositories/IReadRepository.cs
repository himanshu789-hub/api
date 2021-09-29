using System.Collections.Generic;

namespace Shambala.Core.Contracts.Repositories
{
    using Domain;
    using Models.DTOModel;
    using Models;
    using Helphers;
    public interface IReadOutgoingShipmentRepository
    {
        IEnumerable<OutgoingShipment> GetShipmentsBySalesmanIdAndAfterDate(short salesmanId, System.DateTime date);
        //IEnumerable<OutgoingShipmentProductInfoDTO> GetProductsById(int orderId);
        OutgoingShipmentAggregateBLL GetDetails(int Id);
    }
    public interface IReadInvoiceRepository
    {
        InvoiceAggreagateDetailBLL GetAggreate(int outgoingShipmentId, short shopId);
        IEnumerable<InvoiceAggreagateDetailBLL> GetAllInvoiceByShopId(short shopId, System.DateTime? date, InvoiceStatus? status, int page, int? count);
        InvoiceDetailWithInfoBLL GetAllInvoiceDetailOfShopByShipmentId(short shopId, int shipmentId);
        IEnumerable<InvoiceAggreagateDetailBLL> GetNotClearedAggregateByShopIds(short[] shopIds);
    }
    public interface IDebitReadRepository
    {
        IEnumerable<Debit> GetDebitLogs(short shopId, int shipmentId);
    }
}