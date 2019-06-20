namespace Matte.Encoding
{
    using System.Linq;
    using Matte.Bits;

    /// <summary>
    /// Encapsulates coefficients and data in a way that allows fast bitwise XOR operations.
    /// </summary>
    /// <remarks>
    /// Not thread-safe.
    /// </remarks>
    public sealed class Slice
    {
        /// <summary>
        /// The number of booleans that are in <see cref="PackedCoefficients"/>.
        /// </summary>
        public int NumCoefficients { get; }

        /// <summary>
        /// The number of bytes that are in <see cref="PackedData"/>.
        /// </summary>
        public int NumData { get; }

        /// <summary>
        /// The coefficients in a form that allows fast bitwise XOR operations.
        /// </summary>
        public long[] PackedCoefficients { get; }

        /// <summary>
        /// The data in a form that allows fast bitwise XOR operations.
        /// </summary>
        public long[] PackedData { get; }

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
            long[] packedCoefficients,
            long[] packedData)
        {
            NumCoefficients = numCoefficients;
            NumData = numData;
            PackedCoefficients = packedCoefficients;
            PackedData = packedData;
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Slice"/>.
        /// </summary>
        public Slice Clone() =>
            new Slice(
                numCoefficients: NumCoefficients,
                numData: NumData,
                packedCoefficients: PackedCoefficients.ToArray(),
                packedData: PackedData.ToArray()
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