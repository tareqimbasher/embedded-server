using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EmbeddedServer.Strategies
{
    public class ActionResolutionStrategy : IActionResolutionStrategy
    {
        private readonly IActionDescriptorProvider _actionDescriptorProvider;

        public ActionResolutionStrategy(IActionDescriptorProvider actionDescriptorProvider)
        {
            _actionDescriptorProvider = actionDescriptorProvider;
        }

        public ActionDescriptor GetBestMatch(HttpContext httpContext)
        {
            ActionDescriptor bestMatch = null;

            foreach (var descriptor in _actionDescriptorProvider.ActionDescriptors.OrderByDescending(x => x.ActionConstraints.Count))
            {
                bool accepts = false;
                foreach (var constraint in descriptor.ActionConstraints)
                {
                    accepts = constraint.Accept(httpContext, descriptor.MethodInfo);
                    if (!accepts)
                        break;
                }
                if (accepts)
                {
                    if (bestMatch == null)
                    {
                        bestMatch = descriptor;
                    }
                    else
                    {
                        throw new Exception($"Multiple routes match: {httpContext.Uri.PathAndQuery}");
                    }
                }
            }

            if (bestMatch == null)
                throw new Exception("No matching action found.");

            return bestMatch;
        }
    }
}
