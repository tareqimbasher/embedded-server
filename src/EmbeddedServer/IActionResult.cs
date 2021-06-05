using EmbeddedServer.Hosting;
using System.Net;
using System.Threading.Tasks;

namespace EmbeddedServer
{
    public interface IActionResult
    {
        Task WriteToResponseAsync(HttpListenerResponse response, IWebHostConfiguration configuration);
    }
}
