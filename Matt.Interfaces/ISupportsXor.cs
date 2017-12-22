// ReSharper disable UnusedMember.Global
namespace Matt.Interfaces
{
    /// <summary>
    /// Something which supports bitwise XOR operations.
    /// </summary>
    public interface ISupportsXor<in T> where T : ISupportsXor<T>
    {
        /// <summary>
        /// Performs bitwise XOR from the given <typeparamref name="T"/> into this.
        /// </summary>
        void Xor(T from);
    }
}