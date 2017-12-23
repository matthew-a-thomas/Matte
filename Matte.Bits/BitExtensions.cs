namespace Matte.Bits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lists;

    /// <summary>
    /// Various extension methods for dealing with individual bits.
    /// </summary>
    public static class BitExtensions
    {
        /// <summary>
        /// Packs the given enumerable of bytes into the minimum number of <see cref="long"/>s required to store them.
        /// </summary>
        /// <remarks>
        /// Bitwise operations on <see cref="long"/> tend to be significantly faster than on the equivalent number of
        /// <see cref="byte"/>s.
        /// </remarks>
        public static IEnumerable<long> Pack(
            this IEnumerable<byte> bytes) =>
            bytes
                .Buffer(sizeof(long))
                .Select(
                    array =>
                    {
                        if (array.Length == sizeof(long))
                            return array;
                        var newArray = new byte[sizeof(long)];
                        array.CopyTo(
                            newArray,
                            0);
                        return newArray;
                    })
                .Select(array => BitConverter.ToInt64(array, 0));

        /// <summary>
        /// Prepends enough bits to these <paramref name="bits"/> so that their
        /// <see cref="IReadOnlyCollection{T}.Count"/> is an even multiple of eight. 
        /// </summary>
        public static IEnumerable<bool> Pad(
            this IReadOnlyCollection<bool> bits) =>
            Enumerable.Repeat(
                    element: default(bool),
                    count: ((bits.Count - 1) / 8 + 1) * 8 - bits.Count
                )
                .Concat(bits);
        
        /// <summary>
        /// Converts this enumerable of bytes into an enumerable of bits.
        /// </summary>
        public static IEnumerable<bool> ToBits(this IEnumerable<byte> bytes)
        {
            foreach (var @byte in bytes)
            {
                for (byte shift = 0; shift < 8; ++shift)
                    yield return ((@byte << shift) & 0x80) != 0;
            }
        }
        
        /// <summary>
        /// Converts this enumerable of bits into an enumerable of bytes.
        /// </summary>
        public static IEnumerable<byte> ToBytes(this IEnumerable<bool> bits)
        {
            byte shift = 0;
            byte accumulate = 0;
            foreach (var bit in bits)
            {
                accumulate <<= 1;
                if (bit)
                    accumulate |= 1;
                ++shift;

                if (shift != 8)
                    continue;
                
                yield return accumulate;
                accumulate = 0;
                shift = 0;
            }
            if (shift == 0)
                yield break;
            
            while (shift++ != 8)
                accumulate <<= 1;
            yield return accumulate;
        }

        /// <summary>
        /// Unpacks this enumerable of <see cref="long"/>s into equivalent bytes.
        /// </summary>
        /// <remarks>
        /// Note there will be a multiple of 8 bytes returned. Use <see cref="Enumerable.Take{TSource}"/> to trim it
        /// down to size if needed.
        /// </remarks>
        public static IEnumerable<byte> Unpack(this IEnumerable<long> from) =>
            from.SelectMany(BitConverter.GetBytes);
        
        /// <summary>
        /// Modifies the given <see cref="IList{T}"/> by XORing this list into it.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static void XorInto(this IReadOnlyList<bool> from, IList<bool> into) =>
            from.CombineInto(
                into: into,
                combineDelegate: (a, b) => a ^ b
            );
        
        /// <summary>
        /// Modifies the given <see cref="IList{T}"/> by XORing this list into it.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static void XorInto(this IReadOnlyList<byte> from, IList<byte> into) =>
            from.CombineInto(
                into: into,
                combineDelegate: (a, b) => (byte)(a ^ b)
            );
    }
}