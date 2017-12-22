namespace Matt.Bits.Tests
{
    using System.Linq;
    using Bits;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PackedClass
    {
        [TestMethod]
        public void CanGetBackPackedBytes()
        {
            var packed = new Packed(new long[] { 1, 2, 3 });
            var bytes = packed.GetBytes();
            Assert.IsTrue(bytes.SequenceEqual(new byte[]
            {
                1, 0, 0, 0, 0, 0, 0, 0,
                2, 0, 0, 0, 0, 0, 0, 0,
                3, 0, 0, 0, 0, 0, 0, 0
            }));
        }

        [TestMethod]
        public void CanGetBackSameBytes()
        {
            var bytes = new byte[] {0x01, 0x02, 0xF3, 0xE4};
            var packed = Packed.Create(bytes);
            var newBytes = packed.GetBytes().Take(4);
            Assert.IsTrue(newBytes.SequenceEqual(bytes));
        }

        [TestMethod]
        public void ClonesCorrectly()
        {
            var packed = Packed.Create(new byte[] {0x10, 0x01, 0x02, 0xF3});
            var cloned = packed.Clone();
            Assert.IsTrue(packed.GetBytes().SequenceEqual(cloned.GetBytes()));
        }

        [TestMethod]
        public void PacksBitsCorrectly()
        {
            var packed = Packed.Create(new[] {true, false, true});
            Assert.AreEqual(1, packed.PackedBytes.Count);
            Assert.AreEqual(5, packed.PackedBytes[0]);
        }

        [TestMethod]
        public void XorsCorrectly()
        {
            var packed1 = Packed.Create(new byte[] {0x00, 0x01, 0x02, 0x03});
            var packed2 = Packed.Create(new byte[] {0xFF, 0xFF, 0xFF, 0xFF});
            packed1.Xor(packed2);
            Assert.IsTrue(packed1.GetBytes().SequenceEqual(new byte[] {0xFF, 0xFE, 0xFD, 0xFC}.Concat(Enumerable.Repeat(default(byte), 4))));
        }
    }
}