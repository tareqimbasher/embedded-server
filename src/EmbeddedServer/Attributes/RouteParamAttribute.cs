using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmbeddedServer.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RouteParamAttribute : Attribute, IActionConstraint
    {
        public RouteParamAttribute()
        {

        }


        public int Order { get; set; } = 1000;

        public bool Accept(HttpContext httpContext, MethodInfo methodInfo)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (methodInfo == null)
                throw new ArgumentNullException(nameof(methodInfo));


            return true;
        }
    }
}
