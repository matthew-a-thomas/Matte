namespace Matte.Encoding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Matte.Entropy;
    using Matte.Lists;

    /// <summary>
    /// Generates <see cref="Slice"/>s
    /// </summary>
    public class SliceGenerator
    {
        readonly bool _isSystematic;
        readonly Func<IRandom> _randomFactory;

        /// <summary>
        /// Creates a new <see cref="SliceGenerator"/>
        /// </summary>
        /// <param name="isSystematic">
        /// Use true to make the resulting sequence start with <see cref="Slice"/>s of the data in order.
        /// </param>
        /// <param name="randomFactory">Source of entropy</param>
        public SliceGenerator(bool isSystematic, Func<IRandom> randomFactory)
        {
            _isSystematic = isSystematic;
            _randomFactory = randomFactory;
        }

        /// // TODO: This function creates an expensive enumerable--can we use async? or Reactive?
        /// <summary>
        /// Generates an endless sequence of <see cref="Slice"/>s, each of which is a specific combination of the
        /// <paramref name="data"/>
        /// </summary>
        /// <param name="data">The data which will get cloned and mixed in the resulting sequence</param>
        /// <param name="sliceSize">The number of bytes that will go into each slice</param>
        public IEnumerable<Slice> Generate(
            byte[] data,
            int sliceSize)
        {
            using (var random = _randomFactory())
            {
                // Split up the given data into slices of the right size
                var sourceSlices = data.ToSlices(sliceSize)
                    .ToList();

                var result = Enumerable.Concat(
                    // Start with the source slices themselves if this is a systematic generator
                    _isSystematic
                        ? sourceSlices
                        : Enumerable.Empty<Slice>(),

                    // Then follow that up with a never-ending stream of randomly-mixed slices
                    random
                        .ToEndlessBitSequence()
                        .Buffer(sourceSlices.Count)
                        .Where(buffer => buffer.Any(x => x))
                        .Select(sourceSlices.Pick)
                        .Select(x => x.Mix())
                );

                foreach (var element in result)
                    yield return element;
            }
        }
    }
}