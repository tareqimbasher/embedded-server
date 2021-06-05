using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EmbeddedServer.Routing
{
    public class RouteAction
    {
        private readonly HttpMethod _httpMethod;
        private readonly string _route;
        private readonly Func<HttpContext, IActionResult> _action;

        public RouteAction()
        {
        }

        public RouteAction(HttpMethod httpMethod, string route, Func<HttpContext, IActionResult> action)
        {
            _httpMethod = httpMethod;
            _route = route;
            _action = action;
        }


        public RouteAction WithAction(Func<HttpContext, IActionResult> action)
        {
            return this;
        }

        public RouteAction WithAction<TRequestBody>(Func<HttpContext, TRequestBody, IActionResult> action)
        {
            return this;
        }
    }

    public class RouteAction<TRequestBody> : RouteAction
    {
        private readonly HttpMethod _httpMethod;
        private readonly string _route;
        private readonly Func<HttpContext, TRequestBody, IActionResult> _action;

        public RouteAction(HttpMethod httpMethod, string route, Func<HttpContext, TRequestBody, IActionResult> action)
        {
            _httpMethod = httpMethod;
            _route = route;
            _action = action;
        }
    }
}
