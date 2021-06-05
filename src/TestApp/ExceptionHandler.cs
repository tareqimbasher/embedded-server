using EmbeddedServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    public class ExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception exception, HttpContext httpContext = null)
        {
            Console.WriteLine($"An unhandled exception occurred. Details: {exception}");
        }
    }
}
