using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports the HTTP DELETE method.
    /// </summary>
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        private static readonly IReadOnlyCollection<string> _supportedMethods = new[] { "DELETE" };

        /// <summary>
        /// Creates a new <see cref="HttpDeleteAttribute"/>.
        /// </summary>
        //public HttpDeleteAttribute()
        //    : base(_supportedMethods)
        //{
        //}

        /// <summary>
        /// Creates a new <see cref="HttpDeleteAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpDeleteAttribute(string template)
            : base(_supportedMethods, template)
        {
        }
    }
}
