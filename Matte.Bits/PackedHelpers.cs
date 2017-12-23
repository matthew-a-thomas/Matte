namespace Matte.Bits
{
    using System.Collections.Generic;

    /// <summary>
    /// Helper methods for <see cref="Packed"/>.
    /// </summary>
    public static class PackedHelpers
    {
        /// <summary>
        /// Creates a new <see cref="Packed"/> from the given <paramref name="bits"/>.
        /// </summary>
        public static Packed CreateFrom(IReadOnlyCollection<bool> bits) => new Packed(bits.ToLongs());
        
        /// <summary>
        /// Creates a new <see cref="Packed"/> from the given <paramref name="bytes"/>.
        /// </summary>
        public static Packed CreateFrom(IReadOnlyCollection<byte> bytes) => new Packed(bytes.ToLongs());
    }
}