using System;
using Xunit;
using Shambala.Core.Profile;
using Shambala.Core.DTOModels;
using Shambala.Domain;
using AutoMapper;
using System.Collections.Generic;
namespace Shambala.General.Test
{
    public class GeneralTest
    {
        IMapper _mapper;
        public GeneralTest()
        {

            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            _mapper = config.CreateMapper();
        }
        [Fact]
        public void Automapper_Mapping()
        {
            // SalesmanDTO salesmanDTO = new SalesmanDTO { FullName = "Pattrick Beans" };
            // Salesman salesman = _mapper.Map<Salesman>(salesmanDTO);
            // Salesman exptectedMapped = new Salesman() { FullName = "Pattrick Beans" };

            ICollection<ShipmentDTO> dtos = new List<ShipmentDTO>();
            dtos.Add(new ShipmentDTO() { CaretSize = 12, FlavourId = 4, ProductId = 1, TotalDefectPieces = 9, TotalRecievedPieces = 190 });
            dtos.Add(new ShipmentDTO() { CaretSize = 12, FlavourId = 4, ProductId = 1, TotalDefectPieces = 1, TotalRecievedPieces = 190 });
            var result = _mapper.Map<IEnumerable<IncomingShipment>>(dtos);
            Assert.NotNull(result);
            // Assert.Same(exptectedMapped.FullName, salesman.FullName);
            Console.WriteLine("Shipment Domain Value : " + System.Text.Json.JsonSerializer.Serialize(result));
        }
    }
}
