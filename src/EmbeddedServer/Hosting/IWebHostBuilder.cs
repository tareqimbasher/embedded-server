using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Middlewares;
using System;
using System.Net.Http;

namespace EmbeddedServer.Hosting
{
    public interface IWebHostBuilder
    {
        /// <summary>
        /// Can be used to configure services using the root DI container for the <see cref="IWebHost"/> being built.
        /// </summary>
        /// <param name="configureServices">Action to configure services using the root DI container for the <see cref="IWebHost"/> being built.</param>
        IWebHostBuilder ConfigureServices(Action<TinyIoCContainer> configureServices);

        /// <summary>
        /// Specify the urls the <see cref="IWebHost"/> will listen on.
        /// </summary>
        /// <param name="urls">The urls the hosted application will listen on.</param>
        IWebHostBuilder UseUrls(params string[] urls);

        /// <summary>
        /// Add a middleware to the request pipeline on the <see cref="IWebHost"/> being built.
        /// </summary>
        /// <typeparam name="T">Middleware type.</typeparam>
        IWebHostBuilder UseMiddleware<TMiddleware>() where TMiddleware : IMiddleware;

        /// <summary>
        /// An instance of an <see cref="IJsonSerializer"/> that the <see cref="IWebHost"/> will use. This is required.
        /// </summary>
        /// <param name="jsonSerializer"><see cref="IJsonSerializer"/> implementation</param>
        IWebHostBuilder UseJsonSerializer(IJsonSerializer jsonSerializer);

        /// <summary>
        /// An instance of an <see cref="IExceptionHandler"/> that the <see cref="IWebHost"/> will use to report
        /// unhandled exceptions and exceptions in the request pipeline.
        /// </summary>
        IWebHostBuilder UseExceptionHandler(IExceptionHandler exceptionHandler);


        /// <summary>
        /// Builds an <see cref="IWebHost"/> which hosts a web application.
        /// </summary>
        IWebHost Build();
    }
}