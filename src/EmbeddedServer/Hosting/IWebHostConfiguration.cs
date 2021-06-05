using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Middlewares;
using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;

namespace EmbeddedServer.Hosting
{
    /// <summary>
    /// Represents a <see cref="IWebHost"/> configuration.
    /// </summary>
    public interface IWebHostConfiguration
    {
        /// <summary>
        /// If specified, will be called after registering internal services. Used to 
        /// register additional or override existing registrations.
        /// </summary>
        Action<TinyIoCContainer> ConfigureServices { get; set; }

        /// <summary>
        /// Uri prefixes for web server to bind to.
        /// </summary>
        string[] Urls { get; set; }

        /// <summary>
        /// The root IoC container.
        /// </summary>
        TinyIoCContainer RootContainer { get; }

        /// <summary>
        /// Registered middleware types.
        /// </summary>
        MiddlewareCollection Middlewares { get; }

        /// <summary>
        /// Registered JsonSerializer. Can be overridden when building web host using 
        /// the .UseJsonSerializer() extension method.
        /// </summary>
        IJsonSerializer JsonSerializer { get; set; }

        /// <summary>
        /// If set, will be used to handle unhandled exceptions.
        /// </summary>
        IExceptionHandler ExceptionHandler { get; set; }

        List<RouteAction> RouteActions { get; }

        /// <summary>
        /// Ensures this configuration instance is valid.
        /// </summary>
        void Validate();
    }
}