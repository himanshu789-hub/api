using Xunit;
using Xunit.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using Shambala.Core.Models.DTOModel;
using System.Text;

namespace Shambala.Controller.Test
{
    public class IntegrationTest : IClassFixture<TestFixture>
    {
        readonly HttpClient _client;
        readonly ITestOutputHelper _testOutput;
        public IntegrationTest(TestFixture fixture, Xunit.Abstractions.ITestOutputHelper outputHelper)
        {
            _client = fixture.Client;
            _testOutput = outputHelper;
        }
        [Fact]
        public async void GenericControllerWorking_Get()
        {
            var response = await _client.GetAsync("/api/scheme/getbyid/1");
            Assert.True(response.IsSuccessStatusCode);
            string res = await response.Content.ReadAsStringAsync();
            Assert.Empty(res);
        }
        [Fact]
        public async void ShipmentCOntroller_get()
        {
            var response = await _client.GetAsync("/api/shipment/GetProductListByOrderId/1");
            Assert.True(response.IsSuccessStatusCode);

        }
        [Fact]
        public async void SalesmanController_Add()
        {
            SalesmanDTO salesman = new SalesmanDTO
            {
                FullName = "Pattrick Beas",
            };
            var json = System.Text.Json.JsonSerializer.Serialize(salesman);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/salesman/add", data);
            string result = response.Content.ReadAsStringAsync().Result;
            System.Console.WriteLine("Response : " + result);
        }
        [Fact]
        public async void SalesmanController_GetAll()
        {
            var response = await _client.GetAsync("/api/salesman/getall");
            Assert.True(response.IsSuccessStatusCode);

            var result = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("All Salesman" + result);
        }
        [Fact]
        public async void SalesmanController_Update()
        {
            SalesmanDTO salesman = new SalesmanDTO
            {
                FullName = "John Beas",
                Id = 10
            };
            var json = System.Text.Json.JsonSerializer.Serialize(salesman);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync("/api/salesman/update", data);
            Assert.False(response.IsSuccessStatusCode);
        }
        [Fact]
        public async void ProductController_getProductsWithDispatchQuantity()
        {
            var response = await _client.GetAsync("/api/GetProductsWithStockAndDispatch");
            var result = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("Products : ", result);
        }
    }
}
