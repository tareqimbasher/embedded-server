using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EmbeddedServer.Hosting
{
    public class HttpRequestHandler
    {
        private readonly IWebHostConfiguration _configuration;
        private readonly TinyIoCContainer _container;

        public HttpRequestHandler(HttpContext httpContext,
            IWebHostConfiguration configuration,
            TinyIoCContainer container)
        {
            HttpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public HttpContext HttpContext { get; }

        public async Task<IActionResult> GetResultAsync()
        {
            if (!_configuration.Middlewares.Any())
                throw new InvalidOperationException("No middlewares defined.");

            var start = GetMiddleware(0);
            return await start.Invoke(HttpContext);
        }


        public IMiddleware GetMiddleware(int index)
        {
            var current = _configuration.Middlewares[index];

            
            var constructors = current.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            var bestConstructor = constructors
                .Select(x => new
                {
                    Ctor = x,
                    Params = x.GetParameters()
                })
                .Where(x => x.Params.Any(p => p.ParameterType == typeof(RequestDelegate)))
                .OrderByDescending(x => x.Params.Length)
                .FirstOrDefault();

            if (bestConstructor == null)
            {
                throw new Exception($"Middleware must have a constructor that accepts a parameter of type {typeof(RequestDelegate).FullName}.");
            }

            var parameters = new List<object>();
            foreach (var p in bestConstructor.Params)
            {
                if (p.ParameterType == typeof(RequestDelegate))
                {
                    parameters.Add(new RequestDelegate(async (context) =>
                    {
                        if (index + 1 > _configuration.Middlewares.Count)
                        {
                            await GetMiddleware(index + 1).Invoke(HttpContext);
                        }
                    }));
                }
                else
                    parameters.Add(_container.Resolve(p.ParameterType));
            }

            return (IMiddleware)bestConstructor.Ctor.Invoke(parameters.ToArray());
        }
    }
}
