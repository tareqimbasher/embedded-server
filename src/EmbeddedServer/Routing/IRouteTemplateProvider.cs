using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer.Routing
{
    /// <summary>
    /// Interface for attributes which can supply a route template for attribute routing.
    /// </summary>
    public interface IRouteTemplateProvider : IActionConstraint
    {
        /// <summary>
        /// The route template. May be null.
        /// </summary>
        string Template { get; }
    }
}
