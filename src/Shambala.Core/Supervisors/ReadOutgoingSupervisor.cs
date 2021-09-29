using System;
using System.Collections.Generic;
using AutoMapper;
namespace Shambala.Core.Supervisors
{
    using Contracts.Supervisors;
    using Shambala.Core.Models.DTOModel;
    using Contracts.Repositories;
    using Models;
    using Helphers;
    public class ReadOutgoingSupervisor : IReadOutgoingSupervisor
    {
        IReadOutgoingShipmentRepository readOutgoingShipmentRepository;
        IProductRepository productRepository;
        IMapper _mapper;
        public ReadOutgoingSupervisor(IReadOutgoingShipmentRepository readOutgoingShipmentRepository, IMapper mapper, IProductRepository productRepository)
        {
            this.readOutgoingShipmentRepository = readOutgoingShipmentRepository;
            this._mapper = mapper;
            this.productRepository = productRepository;
        }
        public IEnumerable<OutgoingShipmentInfoDTO> GetOutgoingShipmentBySalesmanIdAndAfterDate(short salesmanId, DateTime afterDate)
        {
            return _mapper.Map<IEnumerable<OutgoingShipmentInfoDTO>>(readOutgoingShipmentRepository.GetShipmentsBySalesmanIdAndAfterDate(salesmanId, afterDate));
        }
        public OutgoingShipmentAggregateDTO GetAggregate(int Id)
        {
            OutgoingShipmentAggregateBLL outgoingShipmentAggregate = readOutgoingShipmentRepository.GetDetails(Id);
            OutgoingShipmentAggregateDTO outgoingShipmentAggregateDTO = _mapper.Map<OutgoingShipmentAggregateDTO>(outgoingShipmentAggregate);
            AfterMapper.OutgoingAggregateDetails(outgoingShipmentAggregateDTO.OutgoingShipmentDetails, this.productRepository.GetAllWithNoTracking());
            foreach (OutgoingShipmentAggegateDetailDTO detail in outgoingShipmentAggregateDTO.OutgoingShipmentDetails)
            {
                outgoingShipmentAggregateDTO.TotalNetPrice += detail.NetPrice;
                outgoingShipmentAggregateDTO.TotalSaleQuantity = detail.SchemeInfo.TotalQuantity;
                outgoingShipmentAggregateDTO.TotalShipedPrice += detail.TotalShipedPrice;
                outgoingShipmentAggregateDTO.CustomCaratQuantity += detail.CustomCaratPrices.TotalQuantity;
                outgoingShipmentAggregateDTO.CustomCaratTotalPrice += detail.CustomCaratPrices.TotalPrice;
            }
            return outgoingShipmentAggregateDTO;
        }
    }
}