using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP POST method.
    /// </summary>
    public class HttpPostAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "POST" };

        /// <summary>
        /// Creates a new <see cref="HttpPostAttribute"/>.
        /// </summary>
        //public HttpPostAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpPostAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpPostAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
