﻿namespace Matte.Bits.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PackedExtensionsClass
    {
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
            public void CorrectlyGetsFromFromPacked2()
            {
                var packed = new Packed(new long[] {2});
                Assert.IsFalse(packed.GetLeastSignificantBit(0));
                Assert.IsTrue(packed.GetLeastSignificantBit(1));
            }

            [TestMethod]
            public void CorrectlyGetsFromFromPacked1()
            {
                var packed = new Packed(new long[] {1});
                Assert.IsTrue(packed.GetLeastSignificantBit(0));
            }
        }
    }
}