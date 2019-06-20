namespace Matte.Entropy.Adapters
{
    using System;
    using System.Security.Cryptography;
    using System.Threading;

    /// <summary>
    /// Produces entropy by hashing increasing numbers
    /// </summary>
    public class HashAlgorithmToRandomAdapter : IRandom
    {
        readonly HashAlgorithm _hash;
        long _seed;

        /// <inheritdoc />
        public byte[] Buffer => _hash.Hash;
        
        /// <summary>
        /// Creates a new <see cref="HashAlgorithmToRandomAdapter"/> which produces entropy by hashing increasing values
        /// starting after the given <paramref name="seed"/>
        /// </summary>
        public HashAlgorithmToRandomAdapter(
            HashAlgorithm hash,
            long seed)
        {
            _hash = hash;
            _seed = seed;
        }

        /// <summary>
        /// Disposes of the underlying <see cref="HashAlgorithm"/>
        /// </summary>
        public void Dispose() => _hash.Dispose();

        /// <inheritdoc />
        public void Populate()
        {
            var seed = Interlocked.Increment(ref _seed);
            var bytes = BitConverter.GetBytes(seed);
            _hash.ComputeHash(bytes);
        }
    }
}