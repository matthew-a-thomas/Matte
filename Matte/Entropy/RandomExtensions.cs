namespace Matte.Entropy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography;
    using Matte.Bits;
    using Matte.Entropy.Adapters;

    /// <summary>
    /// Extension methods for <see cref="IRandom"/>.
    /// </summary>
    [SuppressMessage("ReSharper",
        "UnusedMember.Global")]
    public static class RandomExtensions
    {
        /// <summary>
        /// Adapts this <see cref="Random"/> into an <see cref="IRandom"/>.
        /// </summary>
        public static IRandom AsRandom(
            this Random random,
            int bufferSize) => new RandomAdapter(bufferSize, random);

        /// <summary>
        /// Adapts this <see cref="RandomNumberGenerator"/> into an <see cref="IRandom"/>.
        /// </summary>
        public static IRandom AsRandom(
            this RandomNumberGenerator rng,
            int bufferSize) => new RandomNumberGeneratorAdapter(bufferSize, rng);

        /// <summary>
        /// Produces an endless sequence of random bits from this <see cref="IRandom"/>.
        /// </summary>
        [SuppressMessage("ReSharper", "IteratorNeverReturns")]
        public static IEnumerable<bool> ToEndlessBitSequence(this IRandom random)
        {
            while (true)
            {
                random.Populate();
                var bits = random.Buffer.ToBits();
                foreach (var bit in bits)
                    yield return bit;
            }
        }
    }
}