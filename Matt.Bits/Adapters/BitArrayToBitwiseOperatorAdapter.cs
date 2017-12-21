namespace Matt.Bits.Adapters
{
    using System.Collections;

    /// <inheritdoc />
    /// <summary>
    /// Adapts a <see cref="T:System.Collections.BitArray" /> into a <see cref="T:Matt.Bits.IBitwiseOperable`1" />.
    /// </summary>
    public sealed class BitArrayToBitwiseOperatorAdapter : IBitwiseOperable<BitArrayToBitwiseOperatorAdapter>
    {
        private readonly BitArray _bitArray;

        /// <summary>
        /// Creates a new <see cref="BitArrayToBitwiseOperatorAdapter"/>, which adapts a <see cref="BitArray"/> into a
        /// <see cref="IBitwiseOperable{T}"/>.
        /// </summary>
        public BitArrayToBitwiseOperatorAdapter(BitArray bitArray)
        {
            _bitArray = bitArray;
        }

        /// <inheritdoc />
        public void Xor(
            BitArrayToBitwiseOperatorAdapter from)
        {
            _bitArray.Xor(from._bitArray);
        }
    }
}