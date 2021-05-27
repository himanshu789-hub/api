using Shambala.Core.Contracts.Repositories;
using Shambala.Core.Helphers;
using Shambala.Core.Models.DTOModel;
using Shambala.Domain;
using Shambala.Infrastructure;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace Shambala.Repository
{
    using Core.Models.BLLModel;

    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        ShambalaContext _context;
        public InvoiceRepository(ShambalaContext context) : base(context)
        {
            _context = context;
        }       
    }
}