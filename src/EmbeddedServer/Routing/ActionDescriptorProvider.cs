using EmbeddedServer.Attributes;
using EmbeddedServer.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedServer.Routing
{
    /// <summary>
    /// Provides action descriptors.
    /// </summary>
    public class ActionDescriptorProvider : IActionDescriptorProvider
    {
        private readonly IControllerDiscoveryStrategy _controllerDiscoveryStrategy;

        public ActionDescriptorProvider(IControllerDiscoveryStrategy controllerDiscoveryStrategy)
        {
            _controllerDiscoveryStrategy = controllerDiscoveryStrategy;
        }

        public List<ActionDescriptor> ActionDescriptors { get; } = new List<ActionDescriptor>();


        public IActionDescriptorProvider Build()
        {
            var controllerTypes = _controllerDiscoveryStrategy.DiscoverControllers();
            foreach (var controllerType in controllerTypes)
            {
                // Get controller constraints
                var controllerConstraints = controllerType.GetCustomAttributesAs<IActionConstraint>();
                int maxControllerConstraintOrder = controllerConstraints.Any() ? controllerConstraints.Max(x => x.Order) : 0;

                // Get all methods on controller that return an IActionResult or a Task<IActionResult>
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m =>
                    {
                        // If return is IActionResult, include
                        if (typeof(IActionResult).IsAssignableFrom(m.ReturnType))
                            return true;

                        // If return is not a task, exclude
                        if (!typeof(Task).IsAssignableFrom(m.ReturnType))
                            return false;

                        // If return is a task, include only if it has exactly one generic type parameter
                        // and that paramter is a type that implements IActionResult
                        var gargs = m.ReturnType.GenericTypeArguments;
                        if (gargs.Length != 1)
                            return false;

                        return typeof(IActionResult).IsAssignableFrom(gargs.Single());
                    });

                // Get action descriptors
                foreach (var method in methods)
                {
                    var constraints = method.GetCustomAttributesAs<IActionConstraint>();

                    // Process declared constraints
                    if (constraints.Any())
                    {
                        if (controllerConstraints.Any())
                        {
                            int increment = maxControllerConstraintOrder >= int.MaxValue ? int.MaxValue : maxControllerConstraintOrder + 1;
                            if (increment > 0)
                            {
                                constraints.ForEach(x =>
                                {
                                    if (((long)x.Order + (long)increment) >= int.MaxValue == false)
                                        x.Order += increment;
                                });
                            }
                        }
                    }

                    // Create constraints if parameters exist
                    var methodParams = method.GetParameters();
                    if (methodParams.Length > 0)
                    {
                        foreach (var param in methodParams)
                        {
                            constraints.Add(new RouteParamAttribute());
                        }
                    }

                    constraints.AddRange(controllerConstraints);
                    constraints = constraints.OrderBy(x => x.Order).ToList();
                    ActionDescriptors.Add(new ActionDescriptor(controllerType, method, constraints));
                }
            }

            return this;
        }
    }
}
