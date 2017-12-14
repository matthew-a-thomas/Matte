namespace Matt.Bits
{
    using System.Collections.Generic;
    using Lists;

    public static class BitExtensions
    {
        public static IEnumerable<bool> ToBits(this IEnumerable<byte> bytes)
        {
            foreach (var @byte in bytes)
            {
                for (byte shift = 0; shift < 8; ++shift)
                    yield return ((@byte << shift) & 0x80) != 0;
            }
        }
        
        public static IEnumerable<byte> ToBytes(this IEnumerable<bool> bits)
        {
            byte shift = 0;
            byte accumulate = 0;
            foreach (var bit in bits)
            {
                accumulate <<= 1;
                if (bit)
                    accumulate |= 1;
                ++shift;

                if (shift != 8)
                    continue;
                
                yield return accumulate;
                accumulate = 0;
                shift = 0;
            }
            if (shift == 0)
                yield break;
            
            while (shift++ != 8)
                accumulate <<= 1;
            yield return accumulate;
        }
        
        /// <summary>
        /// Modifies the given <see cref="IList{T}"/> by XORing this list into it.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static void XorInto(this IReadOnlyList<bool> from, IList<bool> into) =>
            from.CombineInto(
                into: into,
                combineDelegate: (a, b) => a ^ b
            );
        
        /// <summary>
        /// Modifies the given <see cref="IList{T}"/> by XORing this list into it.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static void XorInto(this IReadOnlyList<byte> from, IList<byte> into) =>
            from.CombineInto(
                into: into,
                combineDelegate: (a, b) => (byte)(a ^ b)
            );
    }
}