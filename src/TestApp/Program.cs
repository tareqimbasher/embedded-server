using EmbeddedServer;
using EmbeddedServer.Hosting;
using EmbeddedServer.Middlewares;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models;

namespace TestApp
{
    internal static class Program
    {
        private static readonly string _uri = "http://localhost:8000/";
        private static HttpClient _testClient;

        public static async Task Main(string[] args)
        {
            try
            {
                var app = WebHost.CreateDefaultBuilder()
                    .UseUrls(_uri)
                    .UseMiddleware<MvcMiddleware>()
                    .UseJsonSerializer(new JsonSerializer())
                    .UseExceptionHandler(new ExceptionHandler())
                    .Build();

                await app.StartAsync();

                Console.WriteLine($"Host started. Listening on {_uri}...");
                string input = null;
                while (input != "exit")
                {
                    Console.WriteLine("\ntype 'test' to run tests | type 'exit' to end");
                    Console.Write("> ");
                    input = Console.ReadLine();
                    if (input == "test")
                        await RunTestsAsync();
                }

                Console.Write("\nShutting down host... ");
                app.Dispose();
                Console.WriteLine("COMPLETE");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Press ENTER to exit ...");
            Console.ReadLine();
        }

        private static async Task RunTestsAsync()
        {
            if (_testClient == null)
            {
                _testClient = new HttpClient
                {
                    Timeout = TimeSpan.FromSeconds(10),
                    BaseAddress = new Uri(_uri)
                };
            }

            try
            {
                Console.WriteLine("\n" + "METHOD".PadRight(15) + "URL".PadRight(50) + "RESULT");
                Console.WriteLine("=======".PadRight(15) + "=====".PadRight(50) + "========");
                await _testClient.GetAsync("products").GetResultAsync();
                await _testClient.GetAsync("products/things").GetResultAsync();
                await _testClient.PostAsync("products/things", null).GetResultAsync();
                await _testClient.DeleteAsync("products/things").GetResultAsync();
                await _testClient.PostAsync("products/things/6/new", new StringContent(
                    JsonConvert.SerializeObject(new Person
                    {
                        Id = 1,
                        Name = "Johnny Appleseed"
                    }), Encoding.UTF8, "application/json"))
                .GetResultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred. Details:\n{ex}");
            }
        }


        private static async Task GetResultAsync(this Task<HttpResponseMessage> task)
        {
            var response = await task;
            var request = response.RequestMessage;

            var json = await (await task).Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ActionResult>(json);

            Console.WriteLine(
                request.Method.Method.PadRight(15)
                + request.RequestUri.PathAndQuery.PadRight(50)
                + (result.IsSuccessful ? "SUCCESS" : "FAILED"));
        }
    }
}
