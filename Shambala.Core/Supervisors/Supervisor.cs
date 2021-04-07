using Shambala.Domain;
using Shambala.Core.DTOModels;
using Shambala.Core.Contracts.Repositories;
using Shambala.Core.Contracts.Supervisors;
using AutoMapper;

namespace Shambala.Core.Supervisors
{
    public class SalesmanSupervisor : GenericSupervisor<Salesman, SalesmanDTO, ISalesmanRepository>, ISalesmanSupervisor
    {

        public SalesmanSupervisor(IMapper mapper, ISalesmanRepository repository) : base(mapper, repository)
        {

        }
    }

    public class SchemeSupervisor : GenericSupervisor<Scheme, SchemeDTO, ISchemeRepository>, ISchemeSupervisor
    {
        public SchemeSupervisor(IMapper mapper, ISchemeRepository repository) : base(mapper, repository)
        {

        }
    }

    public class InvoiceSupervisor : GenericSupervisor<Invoice, InvoiceDTO, IInvoiceRepository>, IInvoiceSupervisor
    {
        public InvoiceSupervisor(IMapper mapper, IInvoiceRepository repository) : base(mapper, repository)
        {

        }
    }
    public class ShopSupervisor : GenericSupervisor<Shop, ShopDTO, IShopRepository>, IShopSupervisor
    {
        public ShopSupervisor(IMapper mapper, IShopRepository repository) : base(mapper, repository)
        {
        }

        public ShopWithInvoicesDTO GetDetailWithInvoices(int Id)
        {
            Shop Shop = _repository.GetWithInvoiceDetail(Id);
            return _mapper.Map<ShopWithInvoicesDTO>(Shop);
        }
    }
}