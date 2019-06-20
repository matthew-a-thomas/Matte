using System;
using Matte.Random;

namespace Matte.Encoding.Fountain
{
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
            var random = _randomFactory();
            var generator = new SliceGenerator(
                isSystematic: true,
                random: random);
            return generator;
        }
    }
}