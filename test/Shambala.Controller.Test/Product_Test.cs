using Xunit;
using System.Net.Http;
using System.Collections.Generic;
using Xunit.Abstractions;
using System.Linq;
namespace Shambala.Controller.Test
{
    using Shambala.Domain;
    using Shambala.Core.Models.DTOModel;
    public class Product_Test : IClassFixture<TestFixture>

    {
        readonly HttpClient _client;
        readonly ITestOutputHelper _testOutput;
        public Product_Test(TestFixture fixture, ITestOutputHelper testOutputHelper)
        {
            this._testOutput = testOutputHelper;
            _client = fixture.Client;
        }
        [Fact]
        public async void ProductGetALl_EnsurePricePerBottleNotNull()
        {
            var result = await _client.GetAsync("/api/product/getall");
            result.EnsureSuccessStatusCode();
            var products = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<ProductDTO>>(await result.Content.ReadAsStringAsync());
            Assert.NotEmpty(products);
            Assert.NotEqual(products.First().PricePerBottle, 0);
        }
    }
}