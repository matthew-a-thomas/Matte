namespace Matte.Tests.Bits
{
    using System.Linq;
    using Matte.Bits;
    using Xunit;

    public class PackedExtensionsClass
    {
        public class GetBitMethod
        {
            [Fact]
            public void GetsLastFourBitsOfPacked0Xf()
            {
                var packed = new long[] {0xF};
                for (var i = 0; i < 4; ++i)
                    Assert.True(packed.GetBit(4, i));
            }
        }

        public class GetBitsMethod
        {
            [Fact]
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
                var packed = bits.ToLongs();
                var newBits = packed.GetBits(bits.Length);
                Assert.True(bits.SequenceEqual(newBits));
            }
        }

        public class GetBytesMethod
        {
            [Fact]
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
                var packed = bytes.ToLongs();
                var newBytes = packed.GetBytes(bytes.Length);
                Assert.True(bytes.SequenceEqual(newBytes));
            }
        }

        public class GetLeastSignificantBitMethod
        {
            [Fact]
            public void CorrectlyGetsAcrossBoundaries()
            {
                var packed = new long[] {1, 2};
                Assert.True(packed.GetLeastSignificantBit(64));
                Assert.True(packed.GetLeastSignificantBit(1));
                Assert.False(packed.GetLeastSignificantBit(0));
            }

            [Fact]
            public void CorrectlyGetsFromFromPacked1()
            {
                var packed = new long[] {1};
                Assert.True(packed.GetLeastSignificantBit(0));
            }

            [Fact]
            public void CorrectlyGetsFromFromPacked2()
            {
                var packed = new long[] {2};
                Assert.False(packed.GetLeastSignificantBit(0));
                Assert.True(packed.GetLeastSignificantBit(1));
            }
        }

        public class GetLeastSignificantByteMethod
        {
            [Fact]
            public void CorrectlyGetsAcrossBoundaries()
            {
                var packed = new long[] {1, 2};
                Assert.Equal(1, packed.GetLeastSignificantByte(8));
                Assert.Equal(2, packed.GetLeastSignificantByte(0));
            }

            [Fact]
            public void CorrectlyGetsFromPacked1()
            {
                var packed = new long[] {1};
                Assert.Equal(1, packed.GetLeastSignificantByte(0));
            }

            [Fact]
            public void CorrectlyGetsFromPacked2()
            {
                var packed = new long[] {2};
                Assert.Equal(2, packed.GetLeastSignificantByte(0));
            }

            [Fact]
            public void CorrectlyGetsFromPacked256()
            {
                var packed = new long[] {256};
                Assert.Equal(0, packed.GetLeastSignificantByte(0));
                Assert.Equal(1, packed.GetLeastSignificantByte(1));
            }
        }
    }
}