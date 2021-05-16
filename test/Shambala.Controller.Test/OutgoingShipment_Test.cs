using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using Shambala.Core.Models.DTOModel;
using System.Text;

namespace Shambala.Controller.Test
{
    public class OutgoingShipment_IntegrationTest : IClassFixture<TestFixture>
    {

        readonly HttpClient _client;
        readonly ITestOutputHelper _testOutput;
        OutgoingShipmentWithSalesmanInfoDTO _outgoingAdded { get; set; }
        public OutgoingShipment_IntegrationTest(TestFixture fixture, Xunit.Abstractions.ITestOutputHelper outputHelper)
        {
            _client = fixture.Client;
            _testOutput = outputHelper;
        }

        [Fact]
        public async void OutgoingShipment_Add()
        {
            PostOutgoingShipmentDTO postOutgoing = new PostOutgoingShipmentDTO
            {
                DateCreated = new System.DateTime(),
                SalesmanId = 1,
                Shipments = new List<ShipmentDTO> { new ShipmentDTO { CaretSize = 30, DateCreated = new System.DateTime(), FlavourId = 1, ProductId = 3, TotalDefectPieces = 2, TotalRecievedPieces = 30 * 2 } }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(postOutgoing);
            //System.Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/shipment/add", data);
            string responseJson = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("Response :=>" + responseJson);
            response.EnsureSuccessStatusCode();
            _outgoingAdded = System.Text.Json.JsonSerializer.Deserialize<OutgoingShipmentWithSalesmanInfoDTO>(responseJson);

        }
        [Fact]
        public async void GetProductListByOrderId()
        {
            int OutgoingShipmentId = 3;
            var response = await _client.GetAsync($"/api/shipment/GetProductListByOrderId/{OutgoingShipmentId}");
            string responseJson = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("Response :=>" + responseJson);
            response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async void OutgoingShipment_Return()
        {
            OutgoingShipmentDetailDTO[] outgoingShipmentDTOs = _outgoingAdded.OutgoingShipmentDetails.ToArray();
            OutgoingShipmentInfoDTO outgoing = new OutgoingShipmentInfoDTO
            {
                Id = _outgoingAdded.Id,
                DateCreated = new System.DateTime(),
                SalesmanId = 1,
                OutgoingShipmentDetails = new List<OutgoingShipmentDetailDTO>(){
                         new OutgoingShipmentDetailDTO{
                             CaretSize=30,
                             FlavourId=outgoingShipmentDTOs[0].FlavourId,
                             ProductId=outgoingShipmentDTOs[0].ProductId,
                             Id=outgoingShipmentDTOs[0].Id,
                             DateCreated=new System.DateTime(),
                             OutgoingShipmentId=outgoingShipmentDTOs[0].OutgoingShipmentId,
                             TotalDefectPieces=2,
                             TotalRecievedPieces=10
                        }}
            };
            var json = System.Text.Json.JsonSerializer.Serialize(outgoing);
            //System.Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("/api/shipment/return", data);
            System.Console.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void OutgoingShipment_Completed()
        {
            int OutgoingShipmentId = _outgoingAdded.Id;
            ICollection<PostInvoiceDTO> postInvoiceDTOs = new List<PostInvoiceDTO>();
            postInvoiceDTOs.Add(new PostInvoiceDTO
            {
                CaretSize = 30,
                DateCreated = new System.DateTime(),
                OutgoingShipmentId = OutgoingShipmentId,
                SchemeId = 1,
                ShopId = 1,
                SoldItems = new List<SoldItemsDTO>{new SoldItemsDTO(){
                 FlavourId=1,ProductId=3,Quantity=10
                }}
            });
            var json = System.Text.Json.JsonSerializer.Serialize(postInvoiceDTOs);
            System.Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"/api/shipment/complete/{OutgoingShipmentId}";
            var response = await _client.PostAsync(url, data);
            System.Console.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void GetOutgoingShipment_SalesmanIdAndDate()
        {
            int salesmanId = 3;
            System.DateTime Date = new System.DateTime(2021, 5, 15);
            var param = new Dictionary<string, string>();
            param.Add("SalesmanId", salesmanId.ToString());
            param.Add("Date", Date.ToString());
            string uri = QueryHelpers.AddQueryString("api/shipment/GetOutgoingBySalesmanIdAndDate", param);
            System.Console.WriteLine("Url : ", uri.ToString());
            var response = await _client.GetAsync(uri);
            System.Console.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
    }
}
