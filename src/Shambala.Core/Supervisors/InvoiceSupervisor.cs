using System.Collections.Generic;
using AutoMapper;
using System;
using System.Transactions;
namespace Shambala.Core.Supervisors
{
    using Core.Helphers;
    using Models.BLLModel;
    using Models.DTOModel;
    using Contracts.Repositories;
    using Contracts.Supervisors;

    public class InvoiceSupervisor : IInvoiceSupervisor
    {
        readonly IReadInvoiceRepository _repository;
        readonly IDebitReadRepository creditReadRepository;
        readonly IMapper _mapper;
        public InvoiceSupervisor(IMapper mapper, IReadInvoiceRepository repository, IDebitReadRepository creditRead)
        {
            this._mapper = mapper;
            this._repository = repository;
            this.creditReadRepository = creditRead;
        }

        public InvoiceBillDTO GetInvoiceBill(int shipmentId, short shopId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<InvoiceDetailDTO> GetInvoiceDetailByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page)
        {
            throw new NotImplementedException();
        }

        public InvoicewithCreditLogDTO GetShopInvoiceWithCreditLog(int shipmentId, short shopId)
        {
            throw new NotImplementedException();
        }

        // public InvoiceBillDTO GetInvoiceBill(int shipmentId, short shopId)
        // {

        //     var option = new TransactionOptions();
        //     option.IsolationLevel = IsolationLevel.RepeatableRead;

        //     using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
        //     {
        //         InvoiceDetailWithInfoBLL invoiceDetailWithInfo = _repository.GetSingleInvoiceAllDetailByShopIdAndShipmentId(shopId, shipmentId);
        //         ShopBillInfo shopBill = _mapper.Map<ShopBillInfo>(invoiceDetailWithInfo);
        //         shopBill.BillingInfoBLLs = this._repository.GetBill(shopId, shipmentId);
        //         return _mapper.Map<InvoiceBillDTO>(shopBill);
        //     }
        // }

        // public IEnumerable<InvoiceDetailDTO> GetInvoiceDetailByShopId(short shopId, DateTime? date, InvoiceStatus? status, int page)
        // {
        //     return _mapper.Map<IEnumerable<InvoiceDetailDTO>>(_repository.GetAllInvoiceByShopId(shopId, date, status, page == 0 ? 1 : page, 15));
        // }
        // public InvoicewithCreditLogDTO GetShopInvoiceWithCreditLog(int shipmentId, short shopId)
        // {
        //     var option = new TransactionOptions();
        //     option.IsolationLevel = IsolationLevel.RepeatableRead;

        //     using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
        //     {
        //         InvoiceDetailWithInfoBLL invoiceDetailWithInfo = _repository.GetAllInvoiceDetailOfShopByShipmentId(shopId, shipmentId);
        //         InvoicewithCreditLogBLL creditLogBLL = _mapper.Map<InvoicewithCreditLogBLL>(invoiceDetailWithInfo);
        //         creditLogBLL.Debits = creditReadRepository.GetDebitLogs(shopId, shipmentId);
        //         return _mapper.Map<InvoicewithCreditLogDTO>(creditLogBLL);
        //     }
        // }

    }
}