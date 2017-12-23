namespace Matte.Bits.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BitExtensionsClass
    {
        [TestClass]
        public class BitsToLongsMethod
        {
            [TestMethod]
            public void PacksTFTInto5()
            {
                var bits = new[] {true, false, true};
                var longs = bits.ToLongs();
                Assert.AreEqual(1, longs.Length);
                Assert.AreEqual(5, longs[0]);
            }

            [TestMethod]
            public void PacksTFInto2()
            {
                var bits = new[] {true, false};
                var longs = bits.ToLongs();
                Assert.AreEqual(1, longs.Length);
                Assert.AreEqual(2, longs[0]);
            }

            [TestMethod]
            public void PacksCorrectlyOverBoundary()
            {
                var bits = Enumerable.Repeat(
                        false,
                        65)
                    .ToArray();
                bits[0] = true;
                bits[64] = true;
                var longs = bits.ToLongs();
                Assert.AreEqual(2, longs.Length);
                Assert.AreEqual(1, longs[0]);
                Assert.AreEqual(1, longs[1]);
            }
        }

        [TestClass]
        public class BytesToLongsMethod
        {
            [TestMethod]
            public void Packs0x1Into1()
            {
                var bytes = new byte[] {0x1};
                var longs = bytes.ToLongs();
                Assert.AreEqual(1, longs.Length);
                Assert.AreEqual(1, longs[0]);
            }

            [TestMethod]
            public void Packs0x05FFInto1535()
            {
                var bytes = new byte[] { 0x05, 0xFF };
                var longs = bytes.ToLongs();
                Assert.AreEqual(1, longs.Length);
                Assert.AreEqual(1535, longs[0]);
            }

            [TestMethod]
            public void PacksCorrectlyOverBoundary()
            {
                var bytes = Enumerable.Repeat(
                        (byte) 0,
                        9)
                    .ToArray();
                bytes[0] = 1;
                bytes[8] = 1;
                var longs = bytes.ToLongs();
                Assert.AreEqual(2, longs.Length);
                Assert.AreEqual(1, longs[0]);
                Assert.AreEqual(1, longs[1]);
            }
        }
    }
}