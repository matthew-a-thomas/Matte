namespace Matte.Bits
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extension methods for arrays of <see cref="long"/>.
    /// </summary>
    public static class PackingExtensions
    {
        /// <summary>
        /// Returns the bit located at the given <paramref name="index"/> within
        /// the last <paramref name="numBits"/> from the end of this array of
        /// <see cref="long"/>.
        /// </summary>
        public static bool GetBit(
            this IReadOnlyList<long> packed,
            int numBits,
            int index) =>
            packed.GetLeastSignificantBit(numBits - index - 1);

        /// <summary>
        /// Gets the given number of bits from this array of <see cref="long"/>.
        /// </summary>
        /// <remarks>
        /// This method takes into account the way that booleans are represented
        /// in an array of <see cref="long"/>.
        /// </remarks>
        public static IEnumerable<bool> GetBits(
            this IReadOnlyList<long> packed,
            int numBits) =>
            Enumerable
                .Range(
                    0,
                    numBits)
                .Select(i => numBits - i - 1)
                .Select(packed.GetLeastSignificantBit);

        /// <summary>
        /// Returns the bit at the given <paramref name="index"/> from the end
        /// of this array of <see cref="long"/>.
        /// </summary>
        public static bool GetLeastSignificantBit(
            this IReadOnlyList<long> packed,
            int index)
        {
            const int numBitsInLong = 64;
            
            var l = packed[packed.Count - index / numBitsInLong - 1];
            return ((l >> index % numBitsInLong) & 1) != 0;
        }

        /// <summary>
        /// Returns the byte at the given <paramref name="index"/> from the end
        /// of this array of <see cref="long"/>.
        /// </summary>
        public static byte GetLeastSignificantByte(
            this IReadOnlyList<long> packed,
            int index)
        {
            const int numBytesInLong = sizeof(long);

            var l = packed[packed.Count - index / numBytesInLong - 1];
            return (byte) ((l >> (index % numBytesInLong) * 8) & 0xFF);
        }

        /// <summary>
        /// Gets the given number of bytes from this array of
        /// <see cref="long"/>.
        /// </summary>
        /// <remarks>
        /// This method takes into account the way that byte arrays are
        /// represented in an array of <see cref="long"/>.
        /// </remarks>
        public static IEnumerable<byte> GetBytes(
            this IReadOnlyList<long> packed,
            int numBytes) =>
            Enumerable
                .Range(
                    0,
                    numBytes)
                .Select(i => numBytes - i - 1)
                .Select(packed.GetLeastSignificantByte);

        /// <summary>
        /// Performs bitwise XOR into this list of <see cref="long"/> from the
        /// given list of <see cref="long"/>.
        /// </summary>
        public static void Xor(
            this IList<long> into,
            IReadOnlyList<long> from)
        {
            if (into == null || from == null)
                return;
            var length = Math.Min(
                into.Count,
                from.Count);
            for (var i = 0; i < length; ++i)
                into[i] ^= from[i];
        }
    }
}