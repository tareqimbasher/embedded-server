using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Middlewares;
using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Hosting
{
    public class WebHostConfiguration : IWebHostConfiguration
    {
        public Action<TinyIoCContainer> ConfigureServices { get; set; }
        public string[] Urls { get; set; }
        public TinyIoCContainer RootContainer { get; } = new TinyIoCContainer();
        public IJsonSerializer JsonSerializer { get; set; }
        public MiddlewareCollection Middlewares { get; } = new MiddlewareCollection();
        public IExceptionHandler ExceptionHandler { get; set; }
        public List<RouteAction> RouteActions { get; } = new List<RouteAction>();


        public void Validate()
        {
            if (Urls == null || Urls.Length == 0)
                throw new Exception("At least one uri prefix is required.");
            if (JsonSerializer == null)
                throw new Exception("A JsonSerializer must be defined.");
        }
    }
}
