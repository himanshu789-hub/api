using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Shambala.Controller.Test
{
    using Shambala.Core.Models.DTOModel;
    public class Credit_IntegrationTest : IClassFixture<TestFixture>
    {
        readonly HttpClient _client;
        readonly ITestOutputHelper _testOutput;
        public Credit_IntegrationTest(TestFixture fixture, ITestOutputHelper outputHelper)
        {
            this._testOutput = outputHelper;
            this._client = fixture.Client;
        }
        [Fact]
        public async void Credit_LeftOver()
        {
            IEnumerable<ShopCreditOrDebitDTO> shopCredits = DTOData.shipmentLedgerDetail.Ledgers.Select(e => new ShopCreditOrDebitDTO
            {
                Amount = e.OldDebit,
                ShopId = e.ShopId
            });


            var json = System.Text.Json.JsonSerializer.Serialize(shopCredits);
            //System.Console.WriteLine(json);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/credit/getleftover", data);
            string responseJson = await response.Content.ReadAsStringAsync();
            System.Console.WriteLine("Response :=>" + responseJson);
            response.EnsureSuccessStatusCode();
        }
    }
}