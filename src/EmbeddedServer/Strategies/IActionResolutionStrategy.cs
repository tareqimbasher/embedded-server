using EmbeddedServer.Routing;

namespace EmbeddedServer.Strategies
{
    public interface IActionResolutionStrategy
    {
        ActionDescriptor GetBestMatch(HttpContext httpContext);
    }
}