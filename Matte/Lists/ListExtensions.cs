namespace Matte.Lists
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Merges the elements from this <see cref="IReadOnlyList{T}"/> into the elements of the given
        /// <see cref="IList{T}"/> using the given <paramref name="combineDelegate"/>.
        /// </summary>
        public static void CombineInto<T>(
            this IReadOnlyList<T> from,
            IList<T> into,
            Func<T, T, T> combineDelegate)
        {
            if (into == null)
                return;
            if (from == null)
                return;
            var length = Math.Min(into.Count, from.Count);
            for (var i = 0; i < length; ++i)
            {
                into[i] = combineDelegate.Invoke(into[i], from[i]);
            }
        }
    }
}