using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EmbeddedServer.Routing
{
    /// <summary>
    /// Describes an endpoint or Action that can be called using HTTP.
    /// </summary>
    public class ActionDescriptor
    {
        public ActionDescriptor()
        {
        }

        public ActionDescriptor(Type controllerType, MethodInfo methodInfo, List<IActionConstraint> actionConstraints)
        {
            ActionConstraints = actionConstraints ?? throw new ArgumentNullException(nameof(actionConstraints));
            ControllerType = controllerType ?? throw new ArgumentNullException(nameof(controllerType));
            MethodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
        }

        public Type ControllerType { get; }
        public MethodInfo MethodInfo { get; set; }
        public List<IActionConstraint> ActionConstraints { get; } = new List<IActionConstraint>();
    }
}
