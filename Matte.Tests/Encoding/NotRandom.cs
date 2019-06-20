namespace Matte.Tests.Encoding
{
    using Matte.Entropy;

    sealed class NotRandom : IRandom
    {
        readonly byte _valueToPopulateWith;

        public NotRandom(byte valueToPopulateWith)
        {
            _valueToPopulateWith = valueToPopulateWith;
        }

        public void Populate(
            byte[] buffer,
            int offset,
            int count)
        {
            count += offset;
            for (; offset < count; ++offset)
                buffer[offset] = _valueToPopulateWith;
        }
    }
}