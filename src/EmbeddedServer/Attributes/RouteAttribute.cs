using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace EmbeddedServer.Attributes
//{
//    /// <summary>
//    /// Specifies an attribute route on a controller.
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
//    public class RouteAttribute : Attribute, IRouteTemplateProvider, IActionConstraint
//    {
//        private int? _order;
//        private string _template;

//        /// <summary>
//        /// Creates a new <see cref="RouteAttribute"/> with the given route template.
//        /// </summary>
//        /// <param name="template">The route template. May not be null.</param>
//        public RouteAttribute(string template)
//        {
//            Template = template ?? throw new ArgumentNullException(nameof(template));
//        }

//        /// <summary>
//        /// The route template. May be null.
//        /// </summary>
//        public string Template { get => _template; protected set => _template = value; }

//        /// <summary>
//        /// Gets the route order. The order determines the order of route execution. Routes with a lower
//        /// order value are tried first. When a route doesn't specify a value, it gets the value of the
//        /// <see cref="RouteAttribute.Order"/> or a default value of 0 if the <see cref="RouteAttribute"/>
//        /// doesn't define a value on the controller.
//        /// </summary>
//        public int Order
//        {
//            get { return _order ?? 0; }
//            set { _order = value; }
//        }

//        public virtual bool Accept(HttpContext httpContext)
//        {
//            if (httpContext == null)
//            {
//                throw new ArgumentNullException(nameof(httpContext));
//            }

//            var templateUri = new Uri($"http://localhost/" + _template);
//            var templateSegments = templateUri.CleanSegments();
//            var requestUrl = httpContext.Request.Url;
//            var requestSegments = requestUrl.CleanSegments();

//            // Request uri must start with template uri
//            if (requestUrl.AbsolutePath.TrimStart('/').StartsWith(_template, StringComparison.OrdinalIgnoreCase) == false)
//                return false;

//            // Template must have atleast as many segments as Request
//            if (templateSegments.Length > requestSegments.Length)
//            {
//                return false;
//            }

//            for (int iSegment = 0; iSegment < templateSegments.Length; iSegment++)
//            {
//                var templateSegment = templateSegments[iSegment];
//                var requestSegment = requestSegments[iSegment];

//                // If segment is not a token, it must match template spelling
//                if (templateSegment.Between("{", "}") == false)
//                {
//                    if (templateSegment.Equals(requestSegment, StringComparison.InvariantCultureIgnoreCase) == false)
//                        return false;
//                }
//            }

//            return true;
//        }
//    }
//}
