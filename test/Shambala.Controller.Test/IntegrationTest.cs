using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
namespace Shambala.Controller.Test
{
    public class IntegrationTest:IClassFixture<TestFixture>
    {
        readonly HttpClient _client;
        public IntegrationTest(TestFixture fixture)
        {
            _client = fixture.Client;
        }
        [Fact]
        public async void GenericControllerWorking_Get()
        {
                    var response = await _client.GetAsync("/api/scheme/getbyid/1");
                    response.EnsureSuccessStatusCode();
        }
    }
}
