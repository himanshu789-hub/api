using System;
using Xunit;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
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
        string _connection = "Server=localhost;Port=3306;Database=shambala;Uid=root;Pwd=sham@12DATA;SslMode=None";
        [Fact]
        public void IncomingShipment_Adding()
        {
            ICollection<IncomingShipmentDTO> dtos = new List<IncomingShipmentDTO>();
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 1, ProductId = 1, TotalDefectPieces = 9, TotalRecievedPieces = 190 });
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 2, ProductId = 1, TotalDefectPieces = 1, TotalRecievedPieces = 190 });
            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            var mapper = config.CreateMapper();
            
            using var logFactory = LoggerFactory.Create(builder => builder.AddNLog("../../../nlog.config").AddConsole());
            var logger = logFactory.CreateLogger<ProductSupervisor>();

            using (var connection = new MySql.Data.MySqlClient.MySqlConnection(_connection))
            {
                var options = new DbContextOptionsBuilder<ShambalaContext>().UseMySQL(connection).Options;
                using (var context = new ShambalaContext(options))
                {
                    using (var unitOfWork = new UnitOfWork.UnitOfWork(context))
                    {
                        var supervisor = new ProductSupervisor(unitOfWork, mapper, logger);
                        supervisor.Add(dtos);
                    }
                }
            }
        }
        [Fact]
        public void Product_All_Getting_Available()
        {

        }
    }
}
