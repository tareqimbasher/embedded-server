using System;
using System.Collections.Generic;

namespace EmbeddedServer.Strategies
{
    public interface IControllerDiscoveryStrategy
    {
        IEnumerable<Type> DiscoverControllers();
    }
}