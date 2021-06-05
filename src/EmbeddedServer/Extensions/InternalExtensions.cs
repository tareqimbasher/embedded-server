using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EmbeddedServer
{
    internal static class InternalExtensions
    {
        internal static string[] CleanUriSegments(this Uri uri) => uri.Segments.Select(x => x.Trim('/'))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();

        /// <summary>
        /// Determines whether the this string instance starts and ends with the specified strings.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startsWithStr">The string to compare to the substring at the start of this instance.</param>
        /// <param name="endsWithStr">The string to compare to the substring at the end of this instance.</param>
        internal static bool Between(this string source, string startsWithStr, string endsWithStr) =>
            source.StartsWith(startsWithStr) && source.EndsWith(endsWithStr);

        internal static List<T> GetCustomAttributesAs<T>(this ICustomAttributeProvider customAttributeProvider)
            where T : class
        {
            return customAttributeProvider
                .GetCustomAttributes(typeof(T), true)
                .Cast<T>()
                .ToList();
        }
    }
}
