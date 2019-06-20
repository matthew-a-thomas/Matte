namespace Matte.Encoding.Fountain
{
    using System;
    using System.Collections.Generic;
    using Bits;

    /// <summary>
    /// Extension methods dealing with <see cref="Slice"/>s.
    /// </summary>
    public static class SliceExtensions
    {
        /// <summary>
        /// Gets the coefficients contained within this <see cref="Slice"/>.
        /// </summary>
        public static IEnumerable<bool> GetCoefficients(this Slice slice) =>
            slice
                .PackedCoefficients
                .GetBits(slice.NumCoefficients);

        /// <summary>
        /// Gets the data contained within this <see cref="Slice"/>.
        /// </summary>
        public static IEnumerable<byte> GetData(this Slice slice) =>
            slice
                .PackedData
                .GetBytes(slice.NumData);

        /// <summary>
        /// XORs together the given <see cref="Slice"/>s into a new
        /// <see cref="Slice"/>.
        /// </summary>
        public static Slice Mix(this IEnumerable<Slice> slices)
        {
            var result = default(Slice);
            var generated = false;
            foreach (var item in slices)
            {
                if (generated)
                {
                    result.Xor(item);
                }
                else
                {
                    generated = true;
                    result = item.Clone();
                }
            }
            return result;
        }

        /// <summary>
        /// Splits the given <paramref name="data"/> up into as many <see cref="Slice"/>s as needed to have slices of
        /// size <paramref name="sliceSize"/>.
        /// </summary>
        public static IEnumerable<Slice> ToSlices(
            this byte[] data,
            int sliceSize)
        {
            var numSlices = (data.Length - 1) / sliceSize + 1;
            for (var i = 0; i < numSlices; ++i)
            {
                var coefficients = new bool[numSlices];
                var sliceData = new byte[sliceSize];
                coefficients[i] = true;
                var sourceIndex = i * sliceSize;
                Array.Copy(
                    sourceArray: data,
                    sourceIndex: sourceIndex,
                    destinationArray: sliceData,
                    destinationIndex: 0,
                    length: Math.Min(data.Length - sourceIndex, sliceSize)
                );
                var slice = SliceHelpers.CreateSlice(
                    coefficients: coefficients,
                    data: sliceData
                );
                yield return slice;
            }
        }
    }
}