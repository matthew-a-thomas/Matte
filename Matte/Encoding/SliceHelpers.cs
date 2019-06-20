namespace Matte.Encoding
{
    using System.Collections.Generic;
    using Matte.Bits;

    /// <summary>
    /// Static helper methods for <see cref="Slice"/>s.
    /// </summary>
    public static class SliceHelpers
    {
        /// <summary>
        /// Calculates the number of slices that would be needed
        /// </summary>
        public static int CalculateNumberOfSlices(
            int totalSize,
            int sliceSize) => (totalSize - 1) / sliceSize + 1;
        
        /// <summary>
        /// Creates a new <see cref="Slice"/> from the given <paramref name="coefficients"/> and
        /// <paramref name="data"/>.
        /// </summary>
        public static Slice CreateSlice(
            IReadOnlyCollection<bool> coefficients,
            IReadOnlyCollection<byte> data) =>
            new Slice(
                numCoefficients: coefficients.Count,
                numData: data.Count,
                packedCoefficients: coefficients.ToLongs(),
                packedData: data.ToLongs()
            );
    }
}