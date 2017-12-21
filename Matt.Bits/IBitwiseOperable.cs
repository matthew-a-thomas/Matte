namespace Matt.Bits
{
    /// <summary>
    /// Something that supports bitwise operations with similar things.
    /// </summary>
    public interface IBitwiseOperable<in T> where T : IBitwiseOperable<T>
    {
        /// <summary>
        /// XORs from the given <typeparamref name="T"/> into this <see cref="IBitwiseOperable{T}"/>, modifying this
        /// <see cref="IBitwiseOperable{T}"/>
        /// </summary>
        void Xor(T from);
    }
}