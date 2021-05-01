using System;
using Xunit;
using Shambala.Core.Profile;
using Shambala.Core.DTOModels;
using Shambala.Domain;
using AutoMapper;
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
            SalesmanDTO salesmanDTO = new SalesmanDTO { FullName = "Pattrick Beans" };
            Salesman salesman = _mapper.Map<Salesman>(salesmanDTO);
            Salesman exptectedMapped = new Salesman() { FullName = "Pattrick Beans" };
            Assert.NotNull(salesman);
            Assert.Same(exptectedMapped.FullName, salesman.FullName);
            Console.WriteLine("Salesman Value : " +System.Text.Json.JsonSerializer.Serialize(salesman));
        }
    }
}
