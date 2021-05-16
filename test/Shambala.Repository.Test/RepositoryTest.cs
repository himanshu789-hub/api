using System;
using Xunit;
using AutoMapper;
using System.Collections.Generic;
using Shambala.Core.Profile;
using System.Text.Json.Serialization;
using Shambala.Core.Models.DTOModel;
using Shambala.Infrastructure;
namespace Shambala.Repository.Test
{
    public class Repository
    {
        IMapper mapper;
        public Repository()
        {
            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            mapper = config.CreateMapper();
        }
        
        [Fact]
        public void OutgoingShipment_GetAllBySalesmanIdAndDate()
        {
            using (var context = new ShambalaContext())
            {
                var repository = new OutgoingShipmentRepository(context);
                var result = repository.GetShipmentsBySalesmnaIdAndDate(3, new System.DateTime(2021, 5, 15,4,5,4));
                System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result));
                
                var response = mapper.Map<IEnumerable<OutgoingShipmentInfoDTO>>(result);
                System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
            }
        }
        [Fact]
        public void OutgoingShipment_GetOrderById()
        {
            using (var context = new ShambalaContext())
            {
                var repository = new OutgoingShipmentRepository(context);
                var result = repository.GetByIdWithNoTracking(3);
                
                var response = mapper.Map<OutgoingShipmentWithSalesmanInfoDTO>(result);
                System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
            }
        }
        
    }
}
