namespace Matte.Bits
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="Packed"/>.
    /// </summary>
    public static class PackedExtensions
    {
        /// <summary>
        /// Returns the bit at the given <paramref name="index"/>.
        /// </summary>
        public static bool GetBit(
            this Packed packed,
            int index)
        {
            const int numBitsInLong = 64;
            
            var l = packed.Contents[index / numBitsInLong];
            return ((l >> index % numBitsInLong) & 1) != 0;
        }
        
        /// <summary>
        /// Gets the given number of bytes from this <see cref="Packed"/>.
        /// </summary>
        /// <remarks>
        /// This method takes into account the way that byte arrays are represented in a <see cref="Packed"/>.
        /// </remarks>
        public static IEnumerable<byte> GetBytes(
            this Packed packed,
            int numBytes) => throw new NotImplementedException();
    }
}