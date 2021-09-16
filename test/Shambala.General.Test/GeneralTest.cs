using System;
using Xunit;
using Shambala.Core.Profile;
using Shambala.Core.Models.DTOModel;
using Shambala.Domain;
using AutoMapper;
using System.Collections.Generic;
using Shambala.Core.Helphers;
namespace Shambala.General.Test
{
    public class GeneralTest
    {
        IMapper _mapper;
        public GeneralTest()
        {

            var config = new MapperConfiguration(opt => opt.AddProfile(new ApplicationProfiles()));
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }
        [Fact]
        public void Automapper_Mapping()
        {
            // SalesmanDTO salesmanDTO = new SalesmanDTO { FullName = "Pattrick Beans" };
            // Salesman salesman = _mapper.Map<Salesman>(salesmanDTO);
            // Salesman exptectedMapped = new Salesman() { FullName = "Pattrick Beans" };

            // ICollection<ShipmentDTO> dtos = new List<ShipmentDTO>();
            // dtos.Add(new ShipmentDTO() { CaretSize = 12, FlavourId = 4, ProductId = 1, TotalDefectPieces = 9, TotalRecievedPieces = 190 });
            //  dtos.Add(new ShipmentDTO() { CaretSize = 12, FlavourId = 4, ProductId = 1, TotalDefectPieces = 1, TotalRecievedPieces = 190 });

            //  ICollection<PostInvoiceDTO> postInvoiceDTOs = new List<PostInvoiceDTO>();
            // postInvoiceDTOs.Add(new PostInvoiceDTO
            // {
            //     CaretSize = 30,
            //     DateCreated = new System.DateTime(),
            //     OutgoingShipmentId = 1,
            //     SchemeId = 1,
            //     ShopId = 1,
            //     SoldItems = new List<SoldItemsDTO>{new SoldItemsDTO(){
            //      FlavourId=1,ProductId=3,Quantity=10
            //     }}
            // });
            // var result = _mapper.Map<IEnumerable<Invoice>>(Utility.ToInvoices(postInvoiceDTOs));
            //            OutgoingShipmentPostDTO outgoing = new OutgoingShipmentPostDTO(){
            //              DateCreated=new System.DateTime(2021,5,13),SalesmanId=1,Shipments=dtos};
            //  OutgoingShipment outgoing = new OutgoingShipment() { DateCreated = new DateTime(2021,5,13), SalesmanIdFk = 10, Id = 333,
            //  Status = "RETURN" };
            //    IncomingShipment incoming = new IncomingShipment(){Id=12,FlavourIdFk=99,ProductIdFk=80};
            //OutgoingShipmentPostDTO outgoing = new OutgoingShipmentPostDTO(){SalesmanId=2};
            var products = new Product() { Id = 1, PricePerCaret = 134.34m, CaretSize = 24, Name = "Demo RGB", SchemeQuantity = 2 };
            var result = _mapper.Map<ProductDTO>(products);
            Assert.NotNull(result);
            Assert.NotEqual(result.PricePerBottle, 0);
            // Assert.Same(exptectedMapped.FullName, salesman.FullName);
            Console.WriteLine("Desiralize Value : " + System.Text.Json.JsonSerializer.Serialize(result));
        }
    }
}
