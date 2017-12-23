namespace Matte.Encoding.Fountain
{
    using System.Diagnostics.CodeAnalysis;
    using Bits;
    using Interfaces;

    /// <summary>
    /// Encapsulates coefficients and data in a way that allows fast bitwise XOR operations.
    /// </summary>
    /// <remarks>
    /// Not thread-safe.
    /// </remarks>
    [SuppressMessage("ReSharper",
        "InheritdocConsiderUsage")]
    public sealed class Slice : ICloneable<Slice>, ISupportsXor<Slice>
    {
        /// <summary>
        /// The number of booleans that are in <see cref="PackedCoefficients"/>.
        /// </summary>
        private readonly int _numCoefficients;

        /// <summary>
        /// The number of bytes that are in <see cref="PackedData"/>.
        /// </summary>
        private readonly int _numData;

        /// <summary>
        /// The coefficients in a form that allows fast bitwise XOR operations.
        /// </summary>
        internal Packed PackedCoefficients { get; }

        /// <summary>
        /// The data in a form that allows fast bitwise XOR operations.
        /// </summary>
        internal Packed PackedData { get; }

        /// <summary>
        /// Creates a new <see cref="Slice"/>.
        /// </summary>
        /// <param name="numCoefficients">
        /// The number of coefficients that are in <paramref name="packedCoefficients"/>.
        /// </param>
        /// <param name="numData">
        /// The number of bytes that are in <paramref name="packedData"/>.
        /// </param>
        /// <param name="packedCoefficients">A compact representation of the coefficients.</param>
        /// <param name="packedData">A compact representation of the data.</param>
        public Slice(
            int numCoefficients,
            int numData,
            Packed packedCoefficients,
            Packed packedData)
        {
            _numCoefficients = numCoefficients;
            _numData = numData;
            PackedCoefficients = packedCoefficients;
            PackedData = packedData;
        }
        
        /// <summary>
        /// Creates an exact copy of this <see cref="Slice"/>.
        /// </summary>
        public Slice Clone() =>
            new Slice(
                numCoefficients: _numCoefficients,
                numData: _numData,
                packedCoefficients: PackedCoefficients.Clone(),
                packedData: PackedData.Clone()
            );
        
        /// <summary>
        /// Quickly performs a bitwise XOR into this <see cref="Slice"/>'s coefficients and data from the given
        /// <see cref="Slice"/>.
        /// </summary>
        public void Xor(
            Slice from)
        {
            PackedCoefficients.Xor(@from.PackedCoefficients);
            PackedData.Xor(@from.PackedData);
        }
    }
}