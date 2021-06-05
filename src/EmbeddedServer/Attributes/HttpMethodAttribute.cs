using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace EmbeddedServer.Attributes
{
    /// <summary>
    /// Identifies an action that only supports a given set of HTTP methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class HttpMethodAttribute : Attribute, IRouteTemplateProvider, IActionConstraint
    {
        private readonly IReadOnlyCollection<string> _httpMethods;
        private readonly string _template;
        private int? _order;

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods.
        /// <param name="httpMethods">The set of supported HTTP methods.</param>
        /// </summary>
        private HttpMethodAttribute(IReadOnlyCollection<string> httpMethods)
        {
            _httpMethods = httpMethods ?? throw new ArgumentNullException(nameof(httpMethods));
        }

        /// <summary>
        /// Creates a new <see cref="HttpMethodAttribute"/> with the given
        /// set of HTTP methods an the given route template.
        /// </summary>
        /// <param name="httpMethods">The set of supported methods.</param>
        /// <param name="template">The route template. May not be null.</param>
        protected HttpMethodAttribute(IReadOnlyCollection<string> httpMethods, string template)
            : this(httpMethods)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }


        /// <summary>
        /// The route template. May be null.
        /// </summary>
        public string Template => _template;

        /// <inheritdoc />
        public IEnumerable<string> HttpMethods => _httpMethods;

        /// <summary>
        /// Gets the route order. The order determines the order of route execution. Routes with a lower
        /// order value are tried first. When a route doesn't specify a value, it gets the value of the
        /// <see cref="RouteAttribute.Order"/> or a default value of 0 if the <see cref="RouteAttribute"/>
        /// doesn't define a value on the controller.
        /// </summary>
        public int Order
        {
            get { return _order ?? 0; }
            set { _order = value; }
        }



        public bool Accept(HttpContext httpContext, MethodInfo methodInfo)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (_httpMethods.Count == 0)
            {
                throw new Exception("No HTTP methods defined");
            }

            // Check if HTTP method is accepted
            var requestMethod = httpContext.Request.HttpMethod;
            if (_httpMethods.Any(x => x.Equals(requestMethod, StringComparison.OrdinalIgnoreCase)) == false)
            {
                return false;
            }

            if (_template != null)
            {
                // Check route template against request uri
                var templateSegments = _template.Trim('/').Split('/');
                var requestSegments = httpContext.Uri.CleanUriSegments();


                if (requestSegments.Length != templateSegments.Length)
                {
                    return false;
                }

                // Compare segments
                for (int iSegment = 0; iSegment < templateSegments.Length; iSegment++)
                {
                    var templateSegment = templateSegments[iSegment];
                    var requestSegment = requestSegments[iSegment];

                    // If segment is not a token, it must match template spelling
                    if (templateSegment.Between("{", "}") == false)
                    {
                        if (templateSegment.Equals(requestSegment, StringComparison.InvariantCultureIgnoreCase) == false)
                            return false;
                    }
                    else
                    {
                        var value = HttpUtility.UrlDecode(requestSegment);
                        var token = templateSegment.Trim('{', '}');

                        var tokenParts = token.Split(':');
                        var tokenName = tokenParts[0];
                        var tokenConstraint = tokenParts.ElementAtOrDefault(1);

                        // Make sure it satisfies type constraint
                        if (tokenConstraint != null)
                        {
                            if (!ValuePassesConstraint(value, tokenConstraint))
                            {
                                return false;
                            }
                        }


                        // Make sure there is a param with the same name
                        var param = methodInfo.GetParameters()
                            .FirstOrDefault(p => p.Name.Equals(tokenName, StringComparison.InvariantCultureIgnoreCase));
                        if (param == null)
                        {
                            return false;
                        }
                    }
                }
            }

            // If we got here, all checks have passed
            return true;
        }


        private bool ValuePassesConstraint(string value, string constraint)
        {
            switch (constraint)
            {
                case "string":
                    return true;
                case "int":
                    return int.TryParse(value, out var i);
                case "long":
                    return long.TryParse(value, out var l);
                case "float":
                    return float.TryParse(value, out var f);
                case "double":
                    return double.TryParse(value, out var d);
                case "decimal":
                    return decimal.TryParse(value, out var m);
                case "bool":
                    return bool.TryParse(value, out var b);
                case "guid":
                    return Guid.TryParse(value, out var g);
                case "alpha":
                    return value == "" || value.All(x => char.IsLetter(x));
                case "datetime":
                    return DateTime.TryParse(value, out var t);
                default:
                    throw new Exception($"Invalid route template segment constraint: {constraint}");
            }
        }
    }
}
