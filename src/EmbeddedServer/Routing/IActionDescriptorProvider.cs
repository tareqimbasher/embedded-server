using EmbeddedServer.Strategies;
using System.Collections.Generic;

namespace EmbeddedServer.Routing
{
    /// <summary>
    /// Interface for attributes which can supply a list of ActionDescriptors.
    /// </summary>
    public interface IActionDescriptorProvider
    {
        List<ActionDescriptor> ActionDescriptors { get; }
        IActionDescriptorProvider Build();
    }
}