using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP OPTIONS method.
    /// </summary>
    public class HttpOptionsAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "OPTIONS" };

        /// <summary>
        /// Creates a new <see cref="HttpOptionsAttribute"/>.
        /// </summary>
        //public HttpOptionsAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpOptionsAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpOptionsAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
