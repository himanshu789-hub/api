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
        readonly ISchemeRepository schemeRepository;
        readonly IShopRepository shopRepository;
        public SchemeSupervisor(IMapper mapper, ISchemeRepository repository, IShopRepository shopRepository) : base(mapper, repository)
        {
            schemeRepository = repository;
            this.shopRepository = shopRepository;
        }

        public IEnumerable<SchemeDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<SchemeDTO>>(schemeRepository.GetAll());
        }

        public SchemeDTO GetByShopId(int shopId)
        {
            Shop shop = this.shopRepository.GetById(shopId);
            if (shop.SchemeIdFk != 0)
                return _mapper.Map<SchemeDTO>(schemeRepository.GetById(shop.SchemeIdFk));
            return null;

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

        public IEnumerable<ShopInfoDTO> GetAllByName(string name)
        {
            return _mapper.Map<IEnumerable<ShopInfoDTO>>(_repository.GetAllByName(name));
        }

        public ShopWithInvoicesDTO GetDetailWithInvoices(int Id)
        {
            Shop Shop = _repository.GetWithInvoiceDetail(Id);
            return _mapper.Map<ShopWithInvoicesDTO>(Shop);
        }
    }
}