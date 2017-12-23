namespace Matte.Bits
{
    using System.Collections.Generic;

    /// <summary>
    /// Various extension methods for dealing with individual bits.
    /// </summary>
    public static class BitExtensions
    {
        /// <summary>
        /// Packs these <paramref name="bits"/> into the fewest number of <see cref="long"/>s needed to store them.
        /// </summary>
        /// <remarks>
        /// The last bit in these <see cref="bits"/> will be the least significant bit of the last <see cref="long"/>
        /// returned from this method.
        /// </remarks>
        public static long[] ToLongs(
            this IReadOnlyCollection<bool> bits)
        {
            const int numBitsInLong = 64;
            var numLongsNeeded = (bits.Count - 1) / numBitsInLong + 1;
            var longs = new long[numLongsNeeded];
            var index = numLongsNeeded * numBitsInLong - bits.Count;
            foreach (var bit in bits)
            {
                if (bit)
                    longs[index / numBitsInLong] |= (long) (1UL << (numBitsInLong - index - 1) % numBitsInLong);
                ++index;
            }

            return longs;
        }

        /// <summary>
        /// Packs these <paramref name="bytes"/> into the fewest number of <see cref="long"/>s needed to store them.
        /// </summary>
        /// <remarks>
        /// The last <see cref="byte"/> in these <see cref="bytes"/> will be placed into the least significant section
        /// of the last <see cref="long"/> returned from this method.
        /// </remarks>
        public static long[] ToLongs(
            this IReadOnlyCollection<byte> bytes)
        {
            const int numBytesInLong = sizeof(long);
            var numLongsNeeded = (bytes.Count - 1) / numBytesInLong + 1;
            var longs = new long[numLongsNeeded];
            var index = numLongsNeeded * numBytesInLong - bytes.Count;
            foreach (var @byte in bytes)
            {
                longs[index / numBytesInLong] |= (long) ((ulong) @byte << ((numBytesInLong - index - 1) % numBytesInLong) * 8);
                ++index;
            }

            return longs;
        }
    }
}