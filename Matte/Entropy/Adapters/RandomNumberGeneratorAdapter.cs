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

        /// <inheritdoc />
        public byte[] Buffer { get; }
        
        /// <summary>
        /// Adapts a <see cref="T:System.Security.Cryptography.RandomNumberGenerator" /> into an <see cref="T:Matte.Entropy.IRandom" />.
        /// </summary>
        public RandomNumberGeneratorAdapter(
            int bufferSize,
            RandomNumberGenerator rng)
        {
            Buffer = new byte[bufferSize];
            _rng = rng;
        }

        /// <summary>
        /// Disposes of the underlying <see cref="RandomNumberGenerator"/>
        /// </summary>
        public void Dispose() => _rng.Dispose();

        /// <inheritdoc />
        public void Populate() => _rng.GetBytes(Buffer);
    }
}