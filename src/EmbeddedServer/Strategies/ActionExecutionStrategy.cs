using EmbeddedServer.DependencyInjection;
using EmbeddedServer.Formatting;
using EmbeddedServer.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EmbeddedServer.Strategies
{
    public class ActionExecutionStrategy : IActionExecutionStrategy
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly TinyIoCContainer _container;

        public ActionExecutionStrategy(IJsonSerializer jsonSerializer, TinyIoCContainer container)
        {
            _jsonSerializer = jsonSerializer;
            _container = container;
        }

        public async Task<IActionResult> ExecuteAsync(HttpContext httpContext, ActionDescriptor actionDescriptor)
        {
            // Create the controller
            var controller = _container.Resolve(actionDescriptor.ControllerType) as Controller;
            controller.HttpContext = httpContext;

            // Get parameters
            var parameters = GetParameters(httpContext, actionDescriptor);

            // Invoke action
            var result = actionDescriptor.MethodInfo.Invoke(controller, parameters);

            if (result is Task task)
            {
                await task.ConfigureAwait(false);
                return (IActionResult)((dynamic)task).Result;
            }
            //if (result is Task task)
            //{
            //    var finalResult = await task;
            //    return finalResult;
            //}
            else
            {
                return result as IActionResult;
            }
        }



        private object[] GetParameters(HttpContext httpContext, ActionDescriptor actionDescriptor)
        {
            var parameters = new List<object>();

            var actionParameterInfos = actionDescriptor.MethodInfo.GetParameters();
            if (actionParameterInfos.Length == 0)
                return parameters.ToArray();

            var queryString = httpContext.Uri.Query ?? "";
            var query = queryString
                .TrimStart('?')
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new
                {
                    Key = x[0],
                    Value = HttpUtility.UrlDecode(x[1])
                })
                .GroupBy(x => x.Key)
                .Select(x => x.Last())
                .ToDictionary(k => k.Key, v => v.Value, StringComparer.InvariantCultureIgnoreCase);



            string content = null;
            using (var stream = new MemoryStream())
            {
                if (httpContext.Request.InputStream != null)
                {
                    httpContext.Request.InputStream.CopyTo(stream);
                    if (stream.Length > 0)
                    {
                        var bytes = stream.ToArray();
                        content = Encoding.UTF8.GetString(bytes);
                    }
                }
            }

            var routeConstraint = actionDescriptor.ActionConstraints.First(x => x is IRouteTemplateProvider) as IRouteTemplateProvider;
            var templateSegments = routeConstraint.Template.Trim('/').Split('/');
            var requestSegments = httpContext.Uri.CleanUriSegments();


            foreach (var pInfo in actionParameterInfos)
            {
                // Try to get value from request url
                string urlValue = null;
                for (int iSegment = 0; iSegment < templateSegments.Length; iSegment++)
                {
                    var templateSegment = templateSegments[iSegment];
                    if (templateSegment.Equals($"{{{pInfo.Name}}}", StringComparison.OrdinalIgnoreCase)
                        || templateSegment.StartsWith($"{{{pInfo.Name}:", StringComparison.OrdinalIgnoreCase))
                    {
                        urlValue = HttpUtility.UrlDecode(requestSegments[iSegment]);
                    }
                }
                if (urlValue != null)
                {
                    parameters.Add(TryParse(urlValue, pInfo.ParameterType));
                }

                // Try to get value from query string
                else if (query.TryGetValue(pInfo.Name, out string queryValue))
                {
                    parameters.Add(TryParse(queryValue, pInfo.ParameterType));
                }

                // Try to get value from content
                else if (!string.IsNullOrWhiteSpace(content))
                {
                    object value = null;
                    try
                    {
                        value = _jsonSerializer.Deserialize(content, pInfo.ParameterType);
                    }
                    catch
                    {
                        // If we can't deserialize content, use null
                        value = null;
                    }
                    parameters.Add(value);
                }

                // Use default value
                else
                {
                    parameters.Add(pInfo.DefaultValue ?? GetDefault(pInfo.ParameterType));
                }
            }

            return parameters.ToArray();
        }

        private object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        private T TryParse<T>(string inValue)
        {
            return (T)TryParse(inValue, typeof(T));
        }

        private object TryParse(string inValue, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromString(null, CultureInfo.InvariantCulture, inValue);
        }
    }
}
