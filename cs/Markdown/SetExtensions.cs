using System.Collections.Generic;

namespace Markdown
{
    internal static class SetExtensions
    {
        internal static void AddRange<T>(this ISet<T> set, IEnumerable<T> enumerable)
        {
            foreach (var element in enumerable)
            {
                set.Add(element);
            }
        }
    }
}
