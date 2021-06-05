using EmbeddedServer.Formatting;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmbeddedServer.Hosting
{
    public class WebServer : IWebServer
    {
        private HttpListener _listener;
        private readonly IWebHostConfiguration _configuration;
        private readonly Func<HttpContext, Task<IActionResult>> _requestReceivedCallback;
        private readonly IJsonSerializer _jsonSerializer;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(4, 25);


        public WebServer(IWebHostConfiguration configuration,
            Func<HttpContext, Task<IActionResult>> requestReceivedCallback,
            IJsonSerializer jsonSerializer)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            if (configuration.Urls == null || configuration.Urls.Length == 0)
                throw new ArgumentException(nameof(configuration.Urls));

            _configuration = configuration;
            _requestReceivedCallback = requestReceivedCallback ?? throw new ArgumentNullException(nameof(requestReceivedCallback));
            _jsonSerializer = jsonSerializer;
        }



        #region Public API

        public async Task<WebServer> StartAsync(CancellationToken cancellationToken)
        {
            if (_listener?.IsListening == true)
                throw new InvalidOperationException($"{nameof(WebServer)} is already started. To restart, you must stop it first, then start it.");

            // Initialize the listener and start it
            _listener = new HttpListener();
            foreach (string s in _configuration.Urls)
                _listener.Prefixes.Add(s);

            _listener.IgnoreWriteExceptions = true;
            _listener.Start();

            while (_listener.IsListening && cancellationToken.IsCancellationRequested == false)
            {
                try
                {
                    await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
                    if (_listener.IsListening && cancellationToken.IsCancellationRequested == false)
                        StartNewHttpListenerContext();
                }
                catch (OperationCanceledException) when(cancellationToken.IsCancellationRequested)
                {
                    _semaphore.Release();
                }
                catch (Exception ex) // suppress any exceptions
                {
                    // TODO log it
                    Console.WriteLine(ex);
                    _semaphore.Release();
                }
            }

            return this;
        }

        public async Task StopAsync()
        {
            if (_listener != null)
            {
                if (_listener.IsListening)
                    _listener.Stop();
                _listener.Close();
                _listener = null;
            }
        }

        #endregion


        #region Private Methods

        private void StartNewHttpListenerContext()
        {
            async Task received(Task<HttpListenerContext> ctxTask)
            {
                _semaphore.Release();
                var listenerContext = await ctxTask;
                var response = listenerContext.Response;

                HttpContext httpContext = null;
                try
                {
                    httpContext = new HttpContext(listenerContext.User, listenerContext.Request, response);

                    IActionResult result = await _requestReceivedCallback(httpContext);
                    await result.WriteToResponseAsync(response, _configuration);
                }
                catch (Exception ex)
                {
                    _configuration.ExceptionHandler?.HandleException(ex, httpContext);
                    await ActionResult.Failed(new ErrorData(ex.Message, ex), HttpStatusCode.InternalServerError)
                        .WriteToResponseAsync(response, _configuration);
                }
                finally
                {
                    // Close the stream
                    response.OutputStream.Dispose();
                    response.Close();
                }
            }

            _listener.GetContextAsync().ContinueWith(received);
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

        ~WebServer()
        {
            Dispose(false);
        }

        #endregion
    }
}
