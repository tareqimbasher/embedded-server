using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Middlewares;
using System;

namespace EmbeddedServer.Hosting
{
    public class DefaultWebHostBuilder : IWebHostBuilder
    {
        private IWebHostConfiguration _configuration = new WebHostConfiguration();


        public IWebHostBuilder ConfigureServices(Action<TinyIoCContainer> configureServices)
        {
            _configuration.ConfigureServices = configureServices;
            return this;
        }


        public IWebHostBuilder UseUrls(params string[] urls)
        {
            _configuration.Urls = urls;
            return this;
        }

        public IWebHostBuilder UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware
        {
            _configuration.Middlewares.Add(typeof(TMiddleware));
            return this;
        }

        public IWebHostBuilder UseMiddleware(Type type)
        {
            if (typeof(IMiddleware).IsAssignableFrom(type) == false)
                throw new ArgumentOutOfRangeException($"Type: {type.FullName} does not implement IMiddleware " +
                    $"and so cannot be used as a middleware.");
            _configuration.Middlewares.Add(type);
            return this;
        }

        public IWebHostBuilder UseJsonSerializer(IJsonSerializer jsonSerializer)
        {
            _configuration.JsonSerializer = jsonSerializer;
            return this;
        }

        public IWebHostBuilder UseExceptionHandler(IExceptionHandler exceptionHandler)
        {
            _configuration.ExceptionHandler = exceptionHandler;
            return this;
        }



        public IWebHost Build()
        {
            _configuration.Validate();
            return new WebHost(_configuration);
        }
    }
}
