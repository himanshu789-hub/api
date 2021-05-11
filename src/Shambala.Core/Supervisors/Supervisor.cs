using Shambala.Domain;
using Shambala.Core.Models.DTOModel;
using Shambala.Core.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using Shambala.Core.Contracts.Supervisors;
using AutoMapper;
using System.Collections.Generic;

namespace Shambala.Core.Supervisors
{
    public class SalesmanSupervisor : GenericSupervisor<Salesman, SalesmanDTO, ISalesmanRepository>, ISalesmanSupervisor
    {
        ILogger<SalesmanSupervisor> _logger;
        public SalesmanSupervisor(IMapper mapper, ISalesmanRepository repository, ILogger<SalesmanSupervisor> logger) : base(mapper, repository)
        {
            _logger = logger;
        }

        public IEnumerable<SalesmanDTO> GetAllActive()
        {
            return _mapper.Map<List<SalesmanDTO>>(_repository.GetAllActive());
        }

    }

    public class SchemeSupervisor : GenericSupervisor<Scheme, SchemeDTO, ISchemeRepository>, ISchemeSupervisor
    {
        public SchemeSupervisor(IMapper mapper, ISchemeRepository repository) : base(mapper, repository)
        {

        }

    }

    public class InvoiceSupervisor : GenericSupervisor<Invoice, PostInvoiceDTO, IInvoiceRepository>, IInvoiceSupervisor
    {
        public InvoiceSupervisor(IMapper mapper, IInvoiceRepository repository) : base(mapper, repository)
        {

        }

    }
    public class IncomingShipmentSupervisor : GenericSupervisor<IncomingShipment, ShipmentDTO, IIncomingShipmentRepository>, IIncomingShipmentSupervisor
    {
        public IncomingShipmentSupervisor(IMapper mapper, IIncomingShipmentRepository repository) : base(mapper, repository)
        {

        }
    }
    public class ShopSupervisor : GenericSupervisor<Shop, ShopDTO, IShopRepository>, IShopSupervisor
    {
        public ShopSupervisor(IMapper mapper, IShopRepository repository) : base(mapper, repository)
        {
        }

        public IEnumerable<ShopDTO> GetAllByName(string name)
        {
            return _mapper.Map<IEnumerable<ShopDTO>>(_repository.GetAllByName(name));
        }

        public ShopWithInvoicesDTO GetDetailWithInvoices(int Id)
        {
            Shop Shop = _repository.GetWithInvoiceDetail(Id);
            return _mapper.Map<ShopWithInvoicesDTO>(Shop);
        }
    }
}