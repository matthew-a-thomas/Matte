namespace Matte.Entropy.Adapters
{
    using System.Security.Cryptography;

    /// <inheritdoc />
    /// <summary>
    /// Adapts a <see cref="T:System.Security.Cryptography.RandomNumberGenerator" /> into an <see cref="T:Matte.Entropy.IRandom" />.
    /// </summary>
    public sealed class RandomNumberGeneratorAdapter : IRandom
    {
        readonly RandomNumberGenerator _rng;

        /// <summary>
        /// Adapts a <see cref="T:System.Security.Cryptography.RandomNumberGenerator" /> into an <see cref="T:Matte.Entropy.IRandom" />.
        /// </summary>
        public RandomNumberGeneratorAdapter(RandomNumberGenerator rng)
        {
            _rng = rng;
        }

        /// <inheritdoc />
        public void Populate(
            byte[] buffer,
            int offset,
            int count) =>
            _rng.GetBytes(buffer, offset, count);
    }
}