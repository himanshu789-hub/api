using AutoMapper;
namespace Shambala.Core.Supervisors
{
    using System.Collections.Generic;
    using Contracts.Supervisors;
    using Shambala.Core.Models.DTOModel;
    using Shambala.Domain;
    using Contracts.UnitOfWork;
    using Contracts.Repositories;
    using Exception;
    public class CreditSupervisor : ICreditSupervisor
    {
        readonly IMapper mapper;
        readonly IUnitOfWork unitOfWork;
        readonly IReadInvoiceRepository readInvoice;
        public CreditSupervisor(IUnitOfWork unitOfWork, IMapper mapper, IReadInvoiceRepository readInvoiceRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.readInvoice = readInvoiceRepository;
        }
        public CreditDTO Add(CreditDTO credit)
        {
            int outgoingShipmentId = credit.OutgoingShipmentId;
            short shopId = credit.ShopId;
            unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
            decimal DuePrice = readInvoice.GetAggreate(outgoingShipmentId, shopId).TotalDuePrice;

            if (credit.Amount > DuePrice)
                throw new CreditFlorishException();

            Credit credit1 = unitOfWork.CreditRepository.Add(credit.OutgoingShipmentId, credit.ShopId, credit.Amount, credit.DateRecieved);
            unitOfWork.SaveChanges();
            return mapper.Map<CreditDTO>(credit1);
        }

        public decimal GetLeftOverCredit(int outgoingShipmentId, short shopId)
        {

            return readInvoice.GetAggreate(outgoingShipmentId, shopId).TotalDuePrice;

        }

        public IEnumerable<CreditDTO> GetLog(int outgoingShipmentId, int shopId)
        {
            var logs = unitOfWork.CreditRepository.FetchList(e => e.OutgoingShipmentIdFk == outgoingShipmentId && e.ShopIdFk == shopId);
            return mapper.Map<IEnumerable<CreditDTO>>(logs);
        }

        public bool IsCreditCleared(int outgoingShipmentId, short shopId)
        {
            return readInvoice.GetAggreate(outgoingShipmentId, shopId).TotalDuePrice == 0;
        }
    }
}