namespace Matte.Entropy.Adapters
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Adapts a <see cref="T:System.Random" /> into an <see cref="T:Matte.Entropy.IRandom" />.
    /// </summary>
    public sealed class RandomAdapter : IRandom
    {
        readonly Random _random;

        /// <summary>
        /// Adapts a <see cref="T:System.Random" /> into an <see cref="T:Matte.Entropy.IRandom" />.
        /// </summary>
        public RandomAdapter(
            int bufferSize,
            Random random)
        {
            Buffer = new byte[bufferSize];
            _random = random;
        }

        void IDisposable.Dispose() {}

        /// <inheritdoc />
        public byte[] Buffer { get; }

        /// <inheritdoc />
        public void Populate() => _random.NextBytes(Buffer);
    }
}