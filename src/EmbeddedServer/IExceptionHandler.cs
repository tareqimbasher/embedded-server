using System;
using System.Collections.Generic;
using System.Text;

namespace EmbeddedServer
{
    public interface IExceptionHandler
    {
        void HandleException(Exception exception, HttpContext httpContext = null);
    }
}
