using EmbeddedServer.Middlewares;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmbeddedServer.Hosting
{
    /// <summary>
    /// Represents a configured web host.
    /// </summary>
    public interface IWebHost : IDisposable
    {
        IWebHostConfiguration Configuration { get; }

        /// <summary>
        /// Runs a web application and kicks off a web server asynchronously.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Attempts to gracefully stop the host and internal web server.
        /// </summary>
        Task StopAsync();
    }
}