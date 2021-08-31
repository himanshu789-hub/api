namespace Shambala.Core.Contracts.Supervisors
{
    using Models.DTOModel;
    using System.Collections.Generic;
    using Helphers;
    public interface IInvoiceSupervisor 
    {
       IEnumerable<InvoiceDetailDTO> GetInvoiceDetailByShopId(short shopId, System.DateTime? date, InvoiceStatus? status, int page);
    //    InvoicewithCreditLogDTO GetShopInvoiceWithCreditLog(int shipmentId, short shopId);
    //    InvoiceBillDTO GetInvoiceBill(int shipmentId, short shopId);
    }
}