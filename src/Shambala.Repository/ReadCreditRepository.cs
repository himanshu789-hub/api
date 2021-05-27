using Shambala.Domain;
using Shambala.Infrastructure;
using Shambala.Core.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shambala.Core.Helphers;
using System.Linq;
using Shambala.Core.Models.BLLModel;
using Shambala.Core.Models.DTOModel;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace Shambala.Repository
{
    public class ReadCreditRepository : ICreditReadRepository
    {
        readonly ShambalaContext context;
        public ReadCreditRepository(ShambalaContext context)
        {
            this.context = context;
        }
        public IEnumerable<Credit> GetCreditLogs(short shopId, int shipmentId)
        {
            var query = context.Credit.Where(e => e.ShopIdFk == shopId && e.OutgoingShipmentIdFk == shipmentId);
            if (context.Database.CurrentTransaction == null && System.Transactions.Transaction.Current == null)
            {
                using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    return query.ToList();
                }
            }
            return query.ToList();
        }
    }
}
