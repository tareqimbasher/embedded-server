using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmbeddedServer.Routing
{
    public interface IActionConstraint
    {
        /// <summary>
        /// The constraint order.
        /// </summary>
        /// <remarks>
        /// Constraints are grouped into stages by the value of <see cref="Order"/>. See remarks on
        /// <see cref="IActionConstraint"/>.
        /// </remarks>
        int Order { get; set; }

        bool Accept(HttpContext httpContext, MethodInfo methodInfo);
    }
}
