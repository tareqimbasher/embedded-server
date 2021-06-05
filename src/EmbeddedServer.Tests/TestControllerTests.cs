using EmbeddedServer;
using EmbeddedServer.Hosting;
using EmbeddedServer.Middlewares;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;

namespace EmbeddedServerTest.Tests
{
    [TestClass]
    public class TestControllerTests
    {
        protected static IWebHost _host;
        protected static HttpClient _client;


        [TestMethod]
        public async Task ActionWithOneSegment()
        {
            var response = await _client.GetAsync("http://localhost:8000/products");

            Assert.IsTrue(response.IsSuccessStatusCode, $"Response did not succeed. HTTP code: {response.StatusCode.ToString()}");
            var json = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(json.Contains("IsSuccess\":true"), $"JSON response was wrong: {json}");
            System.Console.WriteLine(json);
        }

        [TestMethod]
        public async Task ActionWithMultipleSegments()
        {
            var response = await _client.GetAsync("http://localhost:8000/products/first");

            Assert.IsTrue(response.IsSuccessStatusCode, $"Response did not succeed. HTTP code: {response.StatusCode.ToString()}");
            var json = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(json.Contains("IsSuccess\":true"), $"JSON response was wrong: {json}");
            System.Console.WriteLine(json);
        }

        [TestMethod]
        public async Task ActionWithId()
        {
            var response = await _client.GetAsync("http://localhost:8000/products/2");

            Assert.IsTrue(response.IsSuccessStatusCode, $"Response did not succeed. HTTP code: {response.StatusCode.ToString()}");
            var json = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(json.Contains("IsSuccess\":true"), $"JSON response was wrong: {json}");
            System.Console.WriteLine(json);
        }



        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            _host = WebHost.CreateDefaultBuilder()
                .UseUrls("http://localhost:8000/")
                .UseMiddleware<MvcMiddleware>()
                .Build();


            _host.StartAsync();
            _client = new HttpClient();
        }

        [ClassCleanup]
        public static async Task Cleanup()
        {
            _client.Dispose();
            await _host.StopAsync();
        }
    }
}
