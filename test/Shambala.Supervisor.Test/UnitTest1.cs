using System;
using Xunit;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Shambala.Domain;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Shambala.Core.Supervisors;
using NLog.Web;
using Shambala.Core.Profile;
using Shambala.Infrastructure;
using Shambala.Core.DTOModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace Shambala.Supervisor.Test

{
    public class UnitTest1
    {
        ICollection<IncomingShipmentDTO> dtos = new List<IncomingShipmentDTO>();
        public UnitTest1()
        {
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 1, ProductId = 1, TotalDefectPieces = 9, TotalRecievedPieces = 190 });
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 2, ProductId = 1, TotalDefectPieces = 1, TotalRecievedPieces = 190 });

            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            _mapper = config.CreateMapper();
        }
        readonly IMapper _mapper;
        string _connection = "Server=localhost;Port=3306;Database=shambala;Uid=root;Pwd=sham@12DATA;SslMode=None";
        [Fact]
        public void IncomingShipment_Adding()
        {

            using var logFactory = LoggerFactory.Create(builder => builder.AddNLog("../../../nlog.config"));
            var logger = logFactory.CreateLogger<ProductSupervisor>();

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connection))
            {
                var options = new DbContextOptionsBuilder<ShambalaContext>().UseMySQL(connection).Options;
                using (var context = new ShambalaContext(options))
                {
                    using (var unitOfWork = new UnitOfWork.UnitOfWork(context))
                    {
                        var supervisor = new ProductSupervisor(unitOfWork, _mapper, logger);
                        supervisor.Add(dtos);
                    }
                }
            }
        }
        [Fact]
        public void Product_All_Getting_Available()
        {

        }
        [Fact]
        public void IsMappingWorking()
        {
            Assert.NotNull(dtos.First());
            var incomingDto = _mapper.Map<IncomingShipment>(dtos.First());
            Assert.NotNull(incomingDto);
            string serialize = Newtonsoft.Json.JsonConvert.SerializeObject(incomingDto);
            
            using var logFactory = LoggerFactory.Create(builder => builder.AddNLog("../../../nlog.config"));
            var logger = logFactory.CreateLogger<UnitTest1>();
            logger.LogInformation(serialize);

        }
    }
}
