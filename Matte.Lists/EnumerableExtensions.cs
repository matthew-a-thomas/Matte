namespace Matte.Lists
{
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/> instances.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Puts the <paramref name="source"/> sequence into chunks up to size <paramref name="count"/>.
        /// </summary>
        /// <remarks>
        /// The last chunk might not be of size <paramref name="count"/> if the number of items in the
        /// <paramref name="source"/> doesn't evenly divide <paramref name="count"/>.
        /// </remarks>
        public static IEnumerable<T[]> Buffer<T>(
            this IEnumerable<T> source,
            int count)
        {
            var buffer = new List<T>(count);
            foreach (var element in source)
            {
                buffer.Add(element);
                if (buffer.Count != count)
                    continue;
                yield return buffer.ToArray();
                buffer.Clear();
            }
            if (buffer.Count > 0)
                yield return buffer.ToArray();
        }
    }
}