namespace Matte.Bits.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PackedExtensionsClass
    {
        [TestClass]
        public class GetBitMethod
        {
            [TestMethod]
            public void GetsLastFourBitsOfPacked0xF()
            {
                var packed = new Packed(new long[] {0xF});
                for (var i = 0; i < 4; ++i)
                    Assert.IsTrue(packed.GetBit(4, i));
            }
        }

        [TestClass]
        public class GetBitsMethod
        {
            [TestMethod]
            public void GetsSameBitsAsGivenToCreateMethod()
            {
                var bits = new []
                {
                    false,
                    true,
                    true,
                    false,
                    true
                };
                var packed = PackedHelpers.CreateFrom(bits);
                var newBits = packed.GetBits(bits.Length);
                Assert.IsTrue(bits.SequenceEqual(newBits));
            }
        }

        [TestClass]
        public class GetBytesMethod
        {
            [TestMethod]
            public void GetsSameBytesAsGivenToCreateMethod()
            {
                var bytes = new byte[]
                {
                    0x01,
                    0x02,
                    0x03,
                    0x04,
                    0x05,
                    0x06,
                    0x07,
                    0x08,
                    0x09
                };
                var packed = PackedHelpers.CreateFrom(bytes);
                var newBytes = packed.GetBytes(bytes.Length);
                Assert.IsTrue(bytes.SequenceEqual(newBytes));
            }
        }

        [TestClass]
        public class GetLeastSignificantBitMethod
        {
            [TestMethod]
            public void CorrectlyGetsAcrossBoundaries()
            {
                var packed = new Packed(new long[] {1, 2});
                Assert.IsTrue(packed.GetLeastSignificantBit(64));
                Assert.IsTrue(packed.GetLeastSignificantBit(1));
                Assert.IsFalse(packed.GetLeastSignificantBit(0));
            }

            [TestMethod]
            public void CorrectlyGetsFromFromPacked1()
            {
                var packed = new Packed(new long[] {1});
                Assert.IsTrue(packed.GetLeastSignificantBit(0));
            }

            [TestMethod]
            public void CorrectlyGetsFromFromPacked2()
            {
                var packed = new Packed(new long[] {2});
                Assert.IsFalse(packed.GetLeastSignificantBit(0));
                Assert.IsTrue(packed.GetLeastSignificantBit(1));
            }
        }

        [TestClass]
        public class GetLeastSignificantByteMethod
        {
            [TestMethod]
            public void CorrectlyGetsAcrossBoundaries()
            {
                var packed = new Packed(new long[] {1, 2});
                Assert.AreEqual(1, packed.GetLeastSignificantByte(8));
                Assert.AreEqual(2, packed.GetLeastSignificantByte(0));
            }
            
            [TestMethod]
            public void CorrectlyGetsFromPacked1()
            {
                var packed = new Packed(new long[] {1});
                Assert.AreEqual(1, packed.GetLeastSignificantByte(0));
            }
            
            [TestMethod]
            public void CorrectlyGetsFromPacked2()
            {
                var packed = new Packed(new long[] {2});
                Assert.AreEqual(2, packed.GetLeastSignificantByte(0));
            }
            
            [TestMethod]
            public void CorrectlyGetsFromPacked256()
            {
                var packed = new Packed(new long[] {256});
                Assert.AreEqual(0, packed.GetLeastSignificantByte(0));
                Assert.AreEqual(1, packed.GetLeastSignificantByte(1));
            }
        }
    }
}