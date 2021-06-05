using EmbeddedServer.Routing;
using System.Threading.Tasks;

namespace EmbeddedServer.Strategies
{
    public interface IActionExecutionStrategy
    {
        Task<IActionResult> ExecuteAsync(HttpContext httpContext, ActionDescriptor actionDescriptor);
    }
}