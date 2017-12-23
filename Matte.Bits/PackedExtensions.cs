﻿namespace Matte.Bits
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods for <see cref="Packed"/>.
    /// </summary>
    public static class PackedExtensions
    {
        /// <summary>
        /// Gets the given number of bits from this <see cref="Packed"/>.
        /// </summary>
        /// <remarks>
        /// This method takes into account the way that booleans are represented in a <see cref="Packed"/>.
        /// </remarks>
        public static IEnumerable<bool> GetBits(
            this Packed packed,
            int numBits) => throw new NotImplementedException();
        
        /// <summary>
        /// Returns the bit at the given <paramref name="index"/> from the end of this <see cref="Packed"/>.
        /// </summary>
        public static bool GetLeastSignificantBit(
            this Packed packed,
            int index)
        {
            const int numBitsInLong = 64;
            
            var l = packed.Contents[packed.Contents.Count - index / numBitsInLong - 1];
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