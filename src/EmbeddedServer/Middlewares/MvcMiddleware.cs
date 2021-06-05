using EmbeddedServer.Routing;
using EmbeddedServer.Strategies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedServer.Middlewares
{
    /// <summary>
    /// A function that can process an HTTP request.
    /// </summary>
    /// <param name="context">The Microsoft.AspNetCore.Http.HttpContext for the request.</param>
    /// <returns>A task that represents the completion of request processing.</returns>
    public delegate Task RequestDelegate(HttpContext context);

    public class MvcMiddleware : IMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IActionResolutionStrategy _actionResolutionStrategy;
        private readonly IActionExecutionStrategy _actionExecutionStrategy;

        public MvcMiddleware(RequestDelegate next, IActionResolutionStrategy actionResolutionStrategy, IActionExecutionStrategy actionExecutionStrategy)
        {
            _next = next;
            _actionResolutionStrategy = actionResolutionStrategy;
            _actionExecutionStrategy = actionExecutionStrategy;
        }

        public async Task<IActionResult> Invoke(HttpContext httpContext)
        {
            // Find action that matches route
            ActionDescriptor actionDescriptor;

            try
            {
                actionDescriptor = _actionResolutionStrategy.GetBestMatch(httpContext);
            }
            catch (Exception ex)
            {
                return ActionResult.Failed(new ErrorData(ex.Message, ex), System.Net.HttpStatusCode.InternalServerError);
            }

            // Execute an action that matches route
            var result = await _actionExecutionStrategy.ExecuteAsync(httpContext, actionDescriptor);

            // Execute next middleware
            await _next(httpContext);

            // Return result
            return result;
        }
    }
}
