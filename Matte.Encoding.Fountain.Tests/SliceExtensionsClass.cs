namespace Matte.Encoding.Fountain.Tests
{
    using System.Linq;
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
                Assert.Equal(slices.Count, 1);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(10, slices[0].GetData().Count());
            }
            
            [Fact]
            public void CreatesOnlyOneSliceWhenSliceSizeIsSameAsDataLength()
            {
                var data = new byte[5];
                var slices = data.ToSlices(5).ToList();
                Assert.Equal(slices.Count, 1);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(5, slices[0].GetData().Count());
            }
            
            [Fact]
            public void CreatesTwoSlicesWhenSliceSizeIsSlightlySmallerThanDataLength()
            {
                var data = new byte[5];
                var slices = data.ToSlices(4).ToList();
                Assert.Equal(slices.Count, 2);
                Assert.Equal(slices.Count, slices[0].GetCoefficients().Count());
                Assert.Equal(4, slices[0].GetData().Count());
                Assert.Equal(slices.Count, slices[1].GetCoefficients().Count());
                Assert.Equal(4, slices[1].GetData().Count());
            }
        }
    }
}