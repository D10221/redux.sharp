using System;
using System.Linq;
using System.Collections.Generic;

namespace server
{
    static class Extensions
    {
        public static R When<T, R>(this T elements, Func<T, bool> @case, Func<T, R> then, Func<T, R> @else)
        {
            return @case(elements) ? then(elements) : @else(elements);
        }
        public static string Joined(this IEnumerable<string> strings, string separator = "")
        {
            return strings.Aggregate((a, b) => a + separator + b);
        }
        public static IEnumerable<string> NotEmpty(this IEnumerable<string> strings)
        {
            return strings.Where(x => !string.IsNullOrWhiteSpace(x));
        }
        public static T GetService<T>(this IServiceProvider services)
        {
            return (T)services.GetService(typeof(T));
        }
    }
}
