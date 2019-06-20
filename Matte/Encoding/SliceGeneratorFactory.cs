namespace Matte.Encoding
{
    using System;
    using Matte.Entropy;

    /// <summary>
    /// Creates <see cref="SliceGenerator"/>s
    /// </summary>
    public class SliceGeneratorFactory
    {
        readonly Func<IRandom> _randomFactory;

        /// <summary>
        /// Create a new <see cref="SliceGeneratorFactory"/>
        /// </summary>
        public SliceGeneratorFactory(
            Func<IRandom> randomFactory)
        {
            _randomFactory = randomFactory;
        }

        /// <summary>
        /// Creates a new systematic <see cref="SliceGenerator"/>
        /// </summary>
        public SliceGenerator CreateSystematic()
        {
            var generator = new SliceGenerator(
                isSystematic: true,
                randomFactory: _randomFactory);
            return generator;
        }
    }
}