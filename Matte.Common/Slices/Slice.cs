namespace Matte.Common.Slices
{
    using System.Collections.Generic;

    /// <summary>
    /// An immutable piece of data.
    /// </summary>
    public struct Slice
    {
        /// <summary>
        /// Indicates which source slices went into making this
        /// <see cref="Slice"/>.
        /// </summary>
        public readonly IReadOnlyList<bool> Coefficients;

        /// <summary>
        /// This <see cref="Slice"/>'s data.
        /// </summary>
        public readonly IReadOnlyList<byte> Data;

        /// <summary>
        /// Creates a new <see cref="Slice"/>.
        /// </summary>
        public Slice(IReadOnlyList<bool> coefficients, IReadOnlyList<byte> data)
        {
            Coefficients = coefficients;
            Data = data;
        }
    }
}