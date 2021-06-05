using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedServer.Middlewares
{
    public interface IMiddleware
    {
        Task<IActionResult> Invoke(HttpContext httpContext);
    }
}
