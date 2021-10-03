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
            totalNetPrice += ledger.OldCheque;
            return totalNetPrice == ledger.NetPrice;
        }
        public ResultModel Post(LedgerDTO ledger)
        {


            unitOfWork.BeginTransaction(System.Data.IsolationLevel.Serializable);
            OutgoingShipment outgoingShipment = unitOfWork.OutgoingShipmentRepository.GetByIdWithNoTracking(ledger.OutgoingShipmentId);

            if (!this.CheckNet(ledger, outgoingShipment))
            {
                return new ResultModel
                {
                    Name = System.Enum.GetName(typeof(LedgerErrorCode), LedgerErrorCode.NETPRICE_INVALID),
                    Code = ((int)LedgerErrorCode.NETPRICE_INVALID),
                    Content = "Invalid Net Price",
                    IsValid = false
                };
            }
            if (outgoingShipment.RowVersion != ledger.RowVersion)
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
            unitOfWork.OutgoingShipmentRepository.Load(outgoingShipment, e => e.Ledger);
            Ledger ledger1 = new Ledger
            {
                NewCheque = ledger.NewCheque,
                OldCheque = ledger.OldCheque,
                OutgoingShipmentIdFk = outgoingShipment.Id,
                TotalNewChequel = ledger.TotalNewCheque,
                TotalOldCheque = ledger.TotalOldCheque
            };
            if (outgoingShipment.Ledger == null)
            {
                unitOfWork.LedgerRespository.Add(ledger1);
            }
            else
            {
                unitOfWork.LedgerRespository.Update(ledger1);
            }
            unitOfWork.SaveChanges();
            return new ResultModel { IsValid = true };
        }
    }
}