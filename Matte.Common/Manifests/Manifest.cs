namespace Matte.Common.Manifests
{
    using System;

    /// <summary>
    /// Metadata for data and slices.
    /// </summary>
    public struct Manifest
    {
        /// <summary>
        /// A unique identifier for this <see cref="Manifest"/>.
        /// </summary>
        public readonly Guid Id;
        
        /// <summary>
        /// The number of coefficients that each associated slice will have.
        /// </summary>
        public readonly uint NumCoefficients;
        
        /// <summary>
        /// The number of bytes in the data that produced this
        /// <see cref="Manifest"/>.
        /// </summary>
        public readonly uint Size;

        /// <summary>
        /// Creates a new <see cref="Manifest"/>.
        /// </summary>
        public Manifest(Guid id, uint numCoefficients, uint size)
        {
            Id = id;
            NumCoefficients = numCoefficients;
            Size = size;
        }
    }
}