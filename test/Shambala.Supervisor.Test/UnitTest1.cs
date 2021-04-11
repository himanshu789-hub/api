using System;
using Xunit;
using AutoMapper;
using Shambala.UnitOfWork;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Shambala.Core.Supervisors;
using Shambala.Core.Profile;
using Shambala.Infrastructure;
using Shambala.Core.DTOModels;
using Microsoft.EntityFrameworkCore;
namespace Shambala.Supervisor.Test

{
    public class UnitTest1
    {
        [Fact]
        public void IncomingShipment_Adding()
        {
            ICollection<IncomingShipmentDTO> dtos = new List<IncomingShipmentDTO>();
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 1, ProductId = 1, TotalDefectPieces =9, TotalRecievedPieces = 190 });
            dtos.Add(new IncomingShipmentDTO() { CaretSize = 12, FlavourId = 4, Id = 1, ProductId = 1, TotalDefectPieces =1, TotalRecievedPieces = 190 });
            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            var mapper = config.CreateMapper();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<ShambalaContext>().UseSqlite(connection).Options;
            using(var context = new ShambalaContext(options))
            {
                using (var unitOfWork = new UnitOfWork.UnitOfWork(context))
                {
                    var supervisor = new ProductSupervisor(unitOfWork, mapper);
                    supervisor.Add(dtos);
                }
            }
           
        }
    }
}
