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