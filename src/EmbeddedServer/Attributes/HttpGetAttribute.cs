using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP GET method.
    /// </summary>
    public class HttpGetAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "GET" };

        /// <summary>
        /// Creates a new <see cref="HttpGetAttribute"/>.
        /// </summary>
        //public HttpGetAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpGetAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpGetAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
