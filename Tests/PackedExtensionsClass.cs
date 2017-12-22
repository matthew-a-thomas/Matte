namespace Tests
{
    using System.Linq;
    using Matt.Bits;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PackedExtensionsClass
    {
        [TestClass]
        public class GetBitMethod
        {
            [TestMethod]
            public void CanGetBits()
            {
                var packed = new Packed(new [] {1, 1L << 63});
                Assert.IsTrue(packed.GetBit(0));
                Assert.IsTrue(packed.GetBit(64 + 63));
            }

            [TestMethod]
            public void GetSameBitsAsBytes()
            {
                var packed = Packed.Create(new byte[] {0x55, 0xFF});
                var bits =
                    Enumerable.Range(
                            0,
                            16)
                        .Select(x => packed.GetBit(x))
                        .ToList();
                var expectedSequence = new[]
                {
                    true, false, true, false, true, false, true, false,
                    true, true, true, true, true, true, true, true,
                };
                Assert.IsTrue(expectedSequence.SequenceEqual(bits));
            }

            [TestMethod]
            public void GetSameBitsAsWerePutIn()
            {
                var packed = Packed.Create(new[] {true, false, true, false});
                Assert.IsTrue(packed.GetBit(1));
                Assert.IsTrue(packed.GetBit(3));
            }

            [TestMethod]
            public void IsSameEndiannessAsGetBytes()
            {
                var packed = new Packed(new [] {1, 1L << 63});
                var bytes = packed.GetBytes().ToList();
                Assert.AreEqual(1, bytes[0]);
                Assert.AreEqual(0x80, bytes[15]);
            }
        }
    }
}