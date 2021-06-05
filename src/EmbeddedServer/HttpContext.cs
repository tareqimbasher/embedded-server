using System;
using System.Net;
using System.Security.Principal;

namespace EmbeddedServer
{
    public class HttpContext
    {
        public HttpContext(IPrincipal user, HttpListenerRequest request, HttpListenerResponse response)
            : this(request, response)
        {
            User = user;
        }

        public HttpContext(HttpListenerRequest request, HttpListenerResponse response)
        {
            Request = request ?? throw new System.ArgumentNullException(nameof(request));
            Response = response ?? throw new System.ArgumentNullException(nameof(response));
        }


        /// <summary>
        /// Gets the System.Net.HttpListenerRequest that represents a client's request for a resource.
        /// </summary>
        /// <returns>An System.Net.HttpListenerRequest object that represents the client request.</returns>
        public HttpListenerRequest Request { get; }


        /// <summary>
        /// Gets the System.Net.HttpListenerResponse object that will be sent to the client response to the client's request.
        /// </summary>
        /// <returns>An System.Net.HttpListenerResponse object used to send a response back to the client.</returns>
        public HttpListenerResponse Response { get; }

        /// <summary>
        ///    Gets an object used to obtain identity, authentication information, and security
        ///    roles for the client whose request is represented by this System.Net.HttpListenerContext
        ///    object.
        /// </summary>
        /// <returns>
        ///    An System.Security.Principal.IPrincipal object that describes the client, or
        ///    null if the System.Net.HttpListener that supplied this System.Net.HttpListenerContext
        ///    does not require authentication.
        /// </returns>
        public IPrincipal User { get; }

        public Uri Uri => Request.Url;
    }
}