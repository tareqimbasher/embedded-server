using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP PUT method.
    /// </summary>
    public class HttpPutAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "PUT" };

        /// <summary>
        /// Creates a new <see cref="HttpPutAttribute"/>.
        /// </summary>
        //public HttpPutAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpPutAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpPutAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
