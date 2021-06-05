using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Routing;
using EmbeddedServer.Strategies;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmbeddedServer.Hosting
{
    public class WebHost : IWebHost
    {
        // Private fields
        private IWebServer _webServer;
        private CancellationTokenSource _tokenSource;


        // Internal constructor
        internal WebHost(IWebHostConfiguration configuration)
        {
            Configuration = configuration;
        }


        // Public properties
        public IWebHostConfiguration Configuration { get; }



        #region Public API

        /// <summary>
        /// Starts the WebHost
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _tokenSource = new CancellationTokenSource();

            // Configure services
            ConfigureServices(Configuration.RootContainer);
            Configuration.ConfigureServices?.Invoke(Configuration.RootContainer);

            // Start web server asynchronously without waiting for it
            _webServer = Configuration.RootContainer.Resolve<IWebServer>();
            _webServer.StartAsync(_tokenSource.Token);
        }



        public async Task StopAsync()
        {
            // Cancel token
            if (_tokenSource != null)
                _tokenSource.Cancel();

            // Dispose web server
            if (_webServer != null)
                _webServer.Dispose();

            // Dispose root container
            if (Configuration.RootContainer != null)
                Configuration.RootContainer.Dispose();

            // Flag for disposal
            _tokenSource = null;
            _webServer = null;
        }

        #endregion



        #region Private Methods

        private TinyIoCContainer ConfigureServices(TinyIoCContainer container)
        {
            // Register services
            container.Register<IWebHost>(this);
            container.Register<IWebHostConfiguration>(Configuration);
            container.Register<IWebServer, WebServer>();
            container.Register<IJsonSerializer>(Configuration.JsonSerializer);
            container.Register<Func<HttpContext, Task<IActionResult>>>(OnRequestReceived);

            container.Register<IActionExecutionStrategy, ActionExecutionStrategy>().AsMultiInstance();
            container.Register<IActionResolutionStrategy, ActionResolutionStrategy>().AsMultiInstance();
            container.Register<IControllerDiscoveryStrategy, ControllerDiscoveryStrategy>().AsMultiInstance();


            // Register middlewares
            foreach (var type in Configuration.Middlewares)
            {
                container.Register(type).AsMultiInstance();
            }

            // Build ActionDescriptorProvider
            IActionDescriptorProvider actionDescriptorProvider = new ActionDescriptorProvider(container.Resolve<IControllerDiscoveryStrategy>());
            actionDescriptorProvider.Build();
            container.Register<IActionDescriptorProvider>(actionDescriptorProvider);


            // Register controllers
            var controllerTypes = actionDescriptorProvider.ActionDescriptors.Select(x => x.ControllerType).Distinct();
            foreach (var controllerType in controllerTypes)
            {
                Configuration.RootContainer.Register(controllerType);
            }

            return container;
        }

        private async Task<IActionResult> OnRequestReceived(HttpContext httpContext)
        {
            try
            {
                using (var scope = Configuration.RootContainer.GetChildContainer())
                {
                    scope.Register<HttpContext>(httpContext);
                    scope.Register<HttpRequestHandler>().AsMultiInstance();

                    var httpRequestHandler = scope.Resolve<HttpRequestHandler>();

                    return await httpRequestHandler.GetResultAsync();
                }
            }
            catch (Exception ex) when (Configuration.ExceptionHandler != null)
            {
                Configuration.ExceptionHandler.HandleException(ex, httpContext);
                return ActionResult.Failed(new ErrorData(ex));
            }
        }

        #endregion



        #region Static Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWebHostBuilder"/>.
        /// </summary>
        public static IWebHostBuilder CreateDefaultBuilder()
        {
            return new DefaultWebHostBuilder();
        }

        #endregion



        #region Finalization

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopAsync().Wait();
            }
        }

        ~WebHost()
        {
            Dispose(false);
        }

        #endregion
    }
}
