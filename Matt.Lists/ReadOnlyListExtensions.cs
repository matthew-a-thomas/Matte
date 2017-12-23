namespace Matt.Lists
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for <see cref="IReadOnlyList{T}"/> instances.
    /// </summary>
    public static class ReadOnlyListExtensions
    {
        /// <summary>
        /// Selects just the elements from this <see cref="IReadOnlyList{T}"/> which are marked with a "true" in the
        /// given <paramref name="coefficients"/>.
        /// </summary>
        public static IEnumerable<T> Pick<T>(
            this IReadOnlyList<T> from,
            IEnumerable<bool> coefficients) =>
            coefficients
                .Select((x, i) => (Coefficient: x, Index: i))
                .Where(tuple => tuple.Coefficient)
                .Select(tuple => tuple.Index)
                .Select(index => from[index]);
    }
}