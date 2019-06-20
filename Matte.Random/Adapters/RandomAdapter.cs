﻿namespace Matte.Random.Adapters
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Adapts a <see cref="T:System.Random" /> into an <see cref="T:Matte.Random.IRandom" />.
    /// </summary>
    public sealed class RandomAdapter : IRandom
    {
        readonly Random _random;

        /// <summary>
        /// Adapts a <see cref="T:System.Random" /> into an <see cref="T:Matte.Random.IRandom" />.
        /// </summary>
        public RandomAdapter(Random random)
        {
            _random = random;
        }

        /// <inheritdoc />
        public void Populate(
            byte[] buffer,
            int offset,
            int count)
        {
            var temp = new byte[count];
            _random.NextBytes(temp);
            temp.CopyTo(buffer, offset);
        }
    }
}