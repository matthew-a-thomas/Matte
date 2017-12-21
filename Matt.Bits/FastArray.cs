namespace Matt.Bits
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Implements <see cref="IBitwiseOperable{T}"/> using an underlying <see cref="ulong"/> array.
    /// </summary>
    /// <remarks>
    /// This should allow significantly faster bitwise operations than you'd be able to do on plain byte arrays.
    /// </remarks>
    public sealed class FastArray : IBitwiseOperable<FastArray>
    {
        private readonly ulong[] _array;
        private const int BytesPerElement = sizeof(ulong);
        private readonly uint _numBytes;

        private FastArray(
            ulong[] array,
            uint numBytes)
        {
            _array = array;
            _numBytes = numBytes;
        }

        /// <summary>
        /// Creates a new <see cref="FastArray"/> from the given <paramref name="bytes"/>.
        /// </summary>
        public static FastArray FromBytes(
            byte[] bytes)
        {
            var array = new ulong[(bytes.Length - 1) / BytesPerElement + 1];
            var result = new FastArray(
                array: array,
                numBytes: (uint)bytes.Length);
            
            var length = array.Length;
            var doesNotFit = bytes.Length % BytesPerElement != 0;
            if (doesNotFit)
                --length;
            for (var i = 0; i < length; ++i)
            {
                array[i] = BitConverter.ToUInt64(
                    bytes,
                    i * BytesPerElement);
            }
            if (!doesNotFit)
                return result;
            
            var buffer = new byte[BytesPerElement];
            Array.Copy(
                sourceArray: bytes,
                sourceIndex: length * BytesPerElement,
                destinationArray: buffer,
                destinationIndex: 0,
                length: bytes.Length % BytesPerElement);
            array[length] = BitConverter.ToUInt64(
                buffer,
                0);

            return result;
        }

        /// <summary>
        /// Extracts a copy of the underlying bytes from this <see cref="FastArray"/>.
        /// </summary>
        public byte[] ToBytes()
        {
            var result = new byte[_numBytes];
            for (var i = 0; i < _array.Length; ++i)
            {
                Array.Copy(
                    sourceArray: BitConverter.GetBytes(_array[i]),
                    sourceIndex: 0,
                    destinationArray: result,
                    destinationIndex: i * BytesPerElement,
                    length: Math.Min(
                        BytesPerElement,
                        _numBytes - i * BytesPerElement)
                );
            }
            return result;
        }

        /// <inheritdoc />
        public void Xor(
            FastArray from)
        {
            if (_array == null || from._array == null || _numBytes == 0 || from._numBytes == 0)
                return;
            var length = Math.Min(
                from._array.Length,
                _array.Length);
            for (var i = 0; i < length; ++i)
            {
                _array[i] ^= from._array[i];
            }
        }
    }
}