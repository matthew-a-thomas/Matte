namespace Matte.Bits
{
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
            
            var l = packed.PackedBytes[index / numBitsInLong];
            return ((l >> index % numBitsInLong) & 1) != 0;
        }
    }
}