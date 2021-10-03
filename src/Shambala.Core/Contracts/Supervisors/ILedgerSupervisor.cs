using Shambala.Core;
namespace Shambala.Core.Contracts.Supervisors
{

    using Shambala.Core.Models.DTOModel;
    using Shambala.Core.Models;
    public interface ILedgerSupervisor
    {
        ResultModel Post(LedgerDTO ledger);
    }
}