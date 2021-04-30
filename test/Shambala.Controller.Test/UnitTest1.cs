using System;
using Xunit;
using Shambala.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
namespace Shambala.Controller.Test
{
    public class UnitTest1
    {
        [Fact]
        public async void GenericControllerWorking_Add()
        {
            using (var server = new TestServer(new WebHostBuilder().UseStartup<Startup>()))
            {
                using (var client = server.CreateClient())
                {
                    var response = await client.GetAsync("scheme/getbyid/1");
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
