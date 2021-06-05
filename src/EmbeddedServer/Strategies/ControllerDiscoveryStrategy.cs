using System;
using System.Collections.Generic;
using System.Linq;

namespace EmbeddedServer.Strategies
{
    public class ControllerDiscoveryStrategy : IControllerDiscoveryStrategy
    {
        public IEnumerable<Type> DiscoverControllers()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes()
                                  .Where(x => typeof(Controller).IsAssignableFrom(x) && !x.IsAbstract && x.IsPublic && x.IsClass)
                );
        }
    }
}