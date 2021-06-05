using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP HEAD method.
    /// </summary>
    public class HttpHeadAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "HEAD" };

        /// <summary>
        /// Creates a new <see cref="HttpHeadAttribute"/>.
        /// </summary>
        //public HttpHeadAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpHeadAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpHeadAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
