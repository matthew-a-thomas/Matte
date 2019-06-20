namespace Matte.Entropy
{
    using System;

    /// <summary>
    /// Populates arrays with random bytes
    /// </summary>
    public interface IRandom : IDisposable
    {
        /// <summary>
        /// An array of random bytes
        /// </summary>
        /// <remarks>
        /// Make sure you call <see cref="Populate"/> before extracting entropy from here
        /// </remarks>
        byte[] Buffer { get; }
        
        /// <summary>
        /// Populates the <see cref="Buffer"/> with random bytes
        /// </summary>
        void Populate();
    }
}