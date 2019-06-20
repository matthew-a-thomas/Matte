namespace Matte.Entropy
{
    using System;

    /// <summary>
    /// Implementation of <see cref="IRandom"/> that just exposes an injected byte array
    /// </summary>
    public class NotRandom : IRandom
    {
        /// <summary>
        /// Creates a new <see cref="NotRandom"/> that just exposes the given <paramref name="buffer"/>
        /// </summary>
        public NotRandom(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <inheritdoc />
        public byte[] Buffer { get; }
        
        void IDisposable.Dispose() {}

        void IRandom.Populate() {}
    }
}