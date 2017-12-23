namespace Matte.Lists.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumerableExtensionsClass
    {
        [TestClass]
        public class BufferMethod
        {
            [TestMethod]
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
                Assert.AreEqual(2, buffers.Count);
                Assert.IsTrue(buffers[0].Length == 3);
                Assert.IsTrue(buffers[1].Length == 1);
            }

            [TestMethod]
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
                Assert.AreEqual(data.Length, buffers.Count);
                Assert.IsTrue(buffers.All(x => x.Length == 1));
                Assert.IsTrue(buffers.Select(x => x[0]).SequenceEqual(data));
            }
        }
    }
}