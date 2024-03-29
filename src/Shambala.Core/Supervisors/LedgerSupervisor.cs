using System.Linq;
namespace Shambala.Core.Supervisors
{
    using Contracts.Supervisors;
    using Domain;
    using Helphers;
    using Shambala.Core.Models;
    using Shambala.Core.Models.DTOModel;
    using Core.Contracts.UnitOfWork;
    public class LedgerSupervisor : ILedgerSupervisor
    {
        IUnitOfWork unitOfWork;
        IReadOutgoingSupervisor readOutgoingSupervisor;
        public LedgerSupervisor(IUnitOfWork unitOfWork, IReadOutgoingSupervisor readOutgoingSupervisor)
        {
            this.unitOfWork = unitOfWork;
            this.readOutgoingSupervisor = readOutgoingSupervisor;
        }
        private bool CheckNet(LedgerDTO ledger, OutgoingShipment outgoingShipment)
        {
            decimal totalNetPrice = outgoingShipment.OutgoingShipmentDetails.Sum(e => e.NetPrice);
            totalNetPrice += ledger.OldCash;
            totalNetPrice -= ledger.NewCheque;
            totalNetPrice -= ledger.OldCheque;
            return totalNetPrice == ledger.NetPrice;
        }
        public ResultModel Post(LedgerDTO ledgerDTO)
        {

            unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(ledgerDTO.OutgoingShipmentId);
            if (!this.CheckNet(ledgerDTO, outgoingShipment))
            {
                return new ResultModel
                {
                    Name = System.Enum.GetName(typeof(LedgerErrorCode), LedgerErrorCode.NETPRICE_INVALID),
                    Code = ((int)LedgerErrorCode.NETPRICE_INVALID),
                    Content = "Invalid Net Price",
                    IsValid = false
                };
            }
            if (outgoingShipment.RowVersion != ledgerDTO.RowVersion)
            {
                return new ResultModel
                {
                    Code = ((int)ConcurrencyErrorCode.Concurrency_Error),
                    Content = "Another User Alredy Changed",
                    Name = System.Enum.GetName(typeof(ConcurrencyErrorCode), ConcurrencyErrorCode.Concurrency_Error)
                };
            }
            bool IsUpdated = unitOfWork.OutgoingShipmentRepository.Update(outgoingShipment);
            if (!IsUpdated)
            {
                return new ResultModel
                {
                    Code = ((int)ConcurrencyErrorCode.Concurrency_Error),
                    Content = "Concurrency_Error Occured",
                    Name = System.Enum.GetName(typeof(ConcurrencyErrorCode), ConcurrencyErrorCode.Concurrency_Error)
                };
            }
            Ledger prviousShipment = unitOfWork.LedgerRespository.FindByShipmentId(ledgerDTO.OutgoingShipmentId);
            if (prviousShipment == null)
            {

                Ledger ledger1 = new Ledger
                {
                    NewCheque = ledgerDTO.NewCheque,
                    OldCheque = ledgerDTO.OldCheque,
                    OutgoingShipmentIdFk = outgoingShipment.Id,
                    TotalNewCheque = ledgerDTO.TotalNewCheque,
                    TotalOldCheque = ledgerDTO.TotalOldCheque,
                    OldCash=ledgerDTO.OldCash
                };
                unitOfWork.LedgerRespository.Add(ledger1);
            }
            else
            {
                prviousShipment.OldCash  = ledgerDTO.OldCash;
                prviousShipment.NewCheque = ledgerDTO.NewCheque;
                prviousShipment.OldCheque = ledgerDTO.OldCheque;
                prviousShipment.TotalNewCheque = ledgerDTO.TotalNewCheque;
                prviousShipment.TotalOldCheque = ledgerDTO.TotalOldCheque;
                unitOfWork.LedgerRespository.Update(prviousShipment);
            }
            unitOfWork.SaveChanges();
            return new ResultModel { IsValid = true };
        }
    }
}