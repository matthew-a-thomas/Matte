namespace Matte.Tests.Lists
{
    using System.Linq;
    using Matte.Lists;
    using Xunit;

    public class EnumerableExtensionsClass
    {
        public class BufferMethod
        {
            [Fact]
            public void MakesSmallChunkWhenCountDoesNotEvenlyDivide()
            {
                var data = new byte[]
                {
                    0x00,
                    0x01,
                    0x02,
                    0x03
                };
                var buffers = data.Buffer(3)
                    .ToList();
                Assert.Equal(2, buffers.Count);
                Assert.True(buffers[0].Length == 3);
                Assert.True(buffers[1].Length == 1);
            }

            [Fact]
            public void CanBufferSizeOne()
            {
                var data = new byte[]
                {
                    0x00,
                    0x01,
                    0x02,
                    0x03
                };
                var buffers = data.Buffer(1).ToList();
                Assert.Equal(data.Length, buffers.Count);
                Assert.True(buffers.All(x => x.Length == 1));
                Assert.True(buffers.Select(x => x[0]).SequenceEqual(data));
            }
        }
    }
}