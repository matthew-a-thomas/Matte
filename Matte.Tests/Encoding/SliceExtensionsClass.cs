﻿namespace Matte.Tests.Encoding
{
    using System.Linq;
    using Matte.Encoding;
    using Xunit;

    public class SliceExtensionsClass
    {
        public class MixMethod
        {
            [Fact]
            public void ClonesASingleSlice()
            {
                var slice = SliceHelpers.CreateSlice(coefficients: new bool[5], data: new byte[5]);
                var sequence = new [] { slice };
                var mixed = sequence.Mix();
                Assert.Equal(5, mixed.GetCoefficients().Count());
                Assert.Equal(5, mixed.GetData().Count());
            }
        }

        public class ToSlicesMethod
        {
            [Fact]
            public void CreatesOnlyOneSliceWhenSliceSizeIsLargerThanDataLength()
            {
                var data = new byte[5];
                var slices = data.ToSlices(10).ToList();
                Assert.Single(slices);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(10, slices[0].GetData().Count());
            }

            [Fact]
            public void CreatesOnlyOneSliceWhenSliceSizeIsSameAsDataLength()
            {
                var data = new byte[5];
                var slices = data.ToSlices(5).ToList();
                Assert.Single(slices);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(5, slices[0].GetData().Count());
            }

            [Fact]
            public void CreatesTwoSlicesWhenSliceSizeIsSlightlySmallerThanDataLength()
            {
                var data = new byte[5];
                var slices = data.ToSlices(4).ToList();
                Assert.Equal(2, slices.Count);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(4, slices[0].GetData().Count());
                Assert.Equal(slices.Count, slices[1].GetCoefficients().Count());
                Assert.Equal(4, slices[1].GetData().Count());
            }
        }
    }
}