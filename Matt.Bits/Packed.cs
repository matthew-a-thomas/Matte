namespace Matt.Bits
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Interfaces;

    /// <summary>
    /// A non-thread-safe piece of mutable data on which you can quickly perform bitwise XOR.
    /// </summary>
    /// <remarks>
    /// This class represents a tradeoff: it takes time to pack up an array of bytes into an <see cref="T:Matt.Bits.Packed" /> and
    /// to get them back out, but once you do you'll be able to perform bitwise XOR operations orders of magnitude
    /// faster. Preliminary testing suggests that on a 64-bit machine the bitwise XOR operation is ~100x faster on bytes
    /// that have been packed into an array of long[] than the equivalent operation on a plain array of byte[].
    /// </remarks>
    [SuppressMessage("ReSharper",
        "InheritdocConsiderUsage")]
    public sealed class Packed : ICloneable<Packed>, ISupportsXor<Packed>
    {
        /// <summary>
        /// The internal array of <see cref="long"/>s.
        /// </summary>
        public IReadOnlyList<long> PackedBytes => _packedBytes;
        
        /// <summary>
        /// The data stored in a manner that allows fast XOR operations.
        /// </summary>
        private readonly long[] _packedBytes;
        
        /// <summary>
        /// Creates a new non-thread-safe piece of mutable data on which you can quickly perform bitwise XOR.
        /// </summary>
        /// <param name="packedBytes">A compact representation of the contained data. Must be non-null.</param>
        /// <exception cref="ArgumentException"></exception>
        public Packed(long[] packedBytes)
        {
            _packedBytes = packedBytes ?? throw new ArgumentNullException(nameof(packedBytes));
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Packed"/>.
        /// </summary>
        public Packed Clone() => new Packed(_packedBytes.Clone() as long[]);

        /// <summary>
        /// Creates a new <see cref="Packed"/> from the given <paramref name="data"/>.
        /// </summary>
        public static Packed Create(IEnumerable<byte> data) =>
            new Packed(data.Pack().ToArray());

        /// <summary>
        /// Retrieves a copy of the packed data.
        /// </summary>
        /// <remarks>
        /// This will have a multiple of 8 bytes.
        /// </remarks>
        public IEnumerable<byte> GetBytes() => _packedBytes.Unpack();

        /// <summary>
        /// Quickly performs bitwise XOR from the given <see cref="Packed"/> into the contained data.
        /// </summary>
        public void Xor(
            Packed from)
        {
            if (from == null)
                return;
            var length = Math.Min(
                _packedBytes.Length,
                from._packedBytes.Length);
            for (var i = 0; i < length; ++i)
                _packedBytes[i] ^= from._packedBytes[i];
        }
    }
}