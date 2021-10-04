using Shambala.Domain;
using Shambala.Core.Models.DTOModel;
using Shambala.Core.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace Shambala.Core.Supervisors
{
    using System;
    using Contracts.Supervisors;
    using Shambala.Core.Helphers;

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

        public IEnumerable<SalesmanDTO> GetAllByName(string name)
        {
            return _mapper.Map<IEnumerable<SalesmanDTO>>(_repository.FetchList(e => EF.Functions.Like(e.FullName, $"%{name}%")));
        }

        public bool IsNameAlreadyExists(string name, short? Id)
        {
            return _repository.IsNameAlreadyExists(name, Id);
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
        // public ShopWithInvoicesDTO GetDetailWithInvoices(int Id)
        // {
        //     Shop Shop = _repository.GetWithInvoiceDetail(Id);
        //     return _mapper.Map<ShopWithInvoicesDTO>(Shop);
        // }
        public  bool IsNameAlreadyExists(string name, int? Id)
        {
            return _repository.IsNameAlreadyExists(name, Id);
        }
        public IEnumerable<ShopDTO> GetAllByName(string name)
        {
            return _mapper.Map<IEnumerable<ShopDTO>>(_repository.FetchList(e => EF.Functions.Like(e.Title, $"%{name}%")));
        }
    }
}