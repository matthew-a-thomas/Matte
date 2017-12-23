﻿namespace Matte.Encoding.Fountain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extension methods dealing with <see cref="Slice"/>s.
    /// </summary>
    public static class SliceExtensions
    {
        /// <summary>
        /// Gets the coefficients contained within this <see cref="Slice"/>.
        /// </summary>
        public static IReadOnlyCollection<bool> GetCoefficients(
            this Slice slice) => throw new NotImplementedException();

        /// <summary>
        /// Gets the data contained within this <see cref="Slice"/>.
        /// </summary>
        public static IReadOnlyCollection<byte> GetData(
            this Slice slice) => throw new NotImplementedException();
        
        /// <summary>
        /// Splits the given <paramref name="data"/> up into as many <see cref="Slice"/>s as needed to have slices of
        /// size <paramref name="sliceSize"/>.
        /// </summary>
        public static IEnumerable<Slice> ToSlices(
            this byte[] data,
            int sliceSize)
        {
            var numSlices = (data.Length - 1) / sliceSize + 1;
            for (var i = 0; i < numSlices; ++i)
            {
                var coefficients = new bool[numSlices];
                var sliceData = new byte[sliceSize];
                coefficients[i] = true;
                var sourceIndex = i * sliceSize;
                Array.Copy(
                    sourceArray: data,
                    sourceIndex: sourceIndex,
                    destinationArray: sliceData,
                    destinationIndex: 0,
                    length: Math.Min(data.Length - sourceIndex, sliceSize)
                );
                var slice = SliceHelpers.CreateSlice(
                    coefficients: coefficients,
                    data: sliceData
                );
                yield return slice;
            }
        }
    }
}