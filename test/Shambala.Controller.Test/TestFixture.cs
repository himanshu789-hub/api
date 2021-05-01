using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;

namespace Shambala.Controller.Test
{
    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            string root = System.IO.Path.Combine("..", "..", "..", "..", "..", "src", "Shambala");

            var builder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().UseContentRoot(root);
            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:5001");
        }
        public HttpClient Client { get; }
        public TestServer Server { get; }

        public void Dispose()
        {
            Client?.Dispose();
            Server?.Dispose();
        }
    }
}