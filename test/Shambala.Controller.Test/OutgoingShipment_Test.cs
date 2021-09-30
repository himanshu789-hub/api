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
        OutgoingShipmentInfoDTO _outgoingAdded { get; set; }
        public OutgoingShipment_IntegrationTest(TestFixture fixture, Xunit.Abstractions.ITestOutputHelper outputHelper)
        {
            _client = fixture.Client;
            _testOutput = outputHelper;
        }

        [Fact]
        public async void OutgoingShipment_Add()
        {
            OutgoingShipmentPostDTO postOutgoing = new OutgoingShipmentPostDTO
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
            _outgoingAdded = System.Text.Json.JsonSerializer.Deserialize<OutgoingShipmentInfoDTO>(responseJson);

        }

        [Fact]
        public async void OutgoingShipment_CheckAmount()
        {
            var json = System.Text.Json.JsonSerializer.Serialize(DTOData.shipmentLedgerDetail.Ledgers);
            System.Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            string url = $"/api/shipment/checkamount/{DTOData.shipmentLedgerDetail.Id}";
            var response = await _client.PostAsync(url, data);
            System.Console.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void GetOutgoingShipment_SalesmanIdAndDate()
        {
            int salesmanId = 3;
            System.DateTime date = new System.DateTime(2021, 5, 15);
            var param = new Dictionary<string, string>();
            param.Add("SalesmanId", salesmanId.ToString());
            param.Add("Date", date.ToString("s"));
            string uri = QueryHelpers.AddQueryString("api/shipment/GetOutgoingBySalesmanIdAndDate", param).ToString();
            _testOutput.WriteLine("Url : " + uri);
            var response = await _client.GetAsync(uri);
            _testOutput.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async void GetDetailsById()
        {
            int Id = 34;
            var response = await _client.GetAsync("api/shipment/getdetailsbyid/" + 34);
            _testOutput.WriteLine("Response :=>" + await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
        }
    }
}
