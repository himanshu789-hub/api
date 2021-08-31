using AutoMapper;
namespace Shambala.Core.Supervisors
{
    using Core.Helphers;
    using System.Collections.Generic;
    using Contracts.Supervisors;
    using Core.Models;
    using Shambala.Core.Models.DTOModel;
    using Shambala.Domain;
    using Contracts.UnitOfWork;
    using Contracts.Repositories;
    using Exception;
    public class DebitSupervisor : IDebitSupervisor
    {
        readonly IMapper mapper;
        readonly IUnitOfWork unitOfWork;
        readonly IReadInvoiceRepository readInvoice;
        public DebitSupervisor(IUnitOfWork unitOfWork, IMapper mapper, IReadInvoiceRepository readInvoiceRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.readInvoice = readInvoiceRepository;
        }

        public DebitDTO Add(DebitDTO credit)
        {
            throw new System.NotImplementedException();
        }

        public decimal GetLeftOverCredit(int outgoingShipmentId, short shopId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<DebitDTO> GetLog(int outgoingShipmentId, int shopId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsCreditCleared(int outgoingShipmentId, short shopId)
        {
            throw new System.NotImplementedException();
        }
        // public DebitDTO Add(DebitDTO credit)
        // {
        //     int outgoingShipmentId = credit.OutgoingShipmentId;
        //     short shopId = credit.ShopId;
        //     unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        //     decimal DuePrice = readInvoice.GetAggreate(outgoingShipmentId, shopId).TotalDueCleared;
        //     if (credit.Amount > DuePrice)
        //         throw new CreditFlorishException();
        //     Debit credit1 = unitOfWork.DebitRepository.Add(credit.OutgoingShipmentId, credit.ShopId, credit.Amount, credit.DateRecieved);
        //     unitOfWork.SaveChanges();
        //     return mapper.Map<DebitDTO>(credit1);
        // }


        // public decimal GetLeftOverCredit(int outgoingShipmentId, short shopId)
        // {
        //     return readInvoice.GetAggreate(outgoingShipmentId, shopId).TotalDueCleared;
        // }

        // public IEnumerable<DebitDTO> GetLog(int outgoingShipmentId, int shopId)
        // {
        //     var logs = unitOfWork.DebitRepository.FetchList(e => e.OutgoingShipmentIdFk == outgoingShipmentId && e.ShopIdFk == shopId);
        //     return mapper.Map<IEnumerable<DebitDTO>>(logs);
        // }

        // public bool IsCreditCleared(int outgoingShipmentId, short shopId)
        // {
        //     InvoiceAggreagateDetailBLL  detailDTO = readInvoice.GetAggreate(outgoingShipmentId, shopId);
        //     return Utility.IsDueCompleted(detailDTO.TotalDueCleared);
        // }
    }
}