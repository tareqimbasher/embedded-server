using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmbeddedServer.Hosting
{
    public interface IWebServer : IDisposable
    {
        /// <summary>
        /// Starts this web server instance.
        /// </summary>
        /// <param name="cancellationToken">Cancelling this token will stop the web server.</param>
        Task<WebServer> StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Stops this web server instance.
        /// </summary>
        Task StopAsync();
    }
}