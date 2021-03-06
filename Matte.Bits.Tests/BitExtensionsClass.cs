﻿namespace Matte.Bits.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class BitExtensionsClass
    {
        
        public class BitsToLongsMethod
        {
            [Fact]
            public void PacksTFTInto5()
            {
                var bits = new[] {true, false, true};
                var longs = bits.ToLongs();
                Assert.Equal(1, longs.Length);
                Assert.Equal(5, longs[0]);
            }

            [Fact]
            public void PacksTFInto2()
            {
                var bits = new[] {true, false};
                var longs = bits.ToLongs();
                Assert.Equal(1, longs.Length);
                Assert.Equal(2, longs[0]);
            }

            [Fact]
            public void PacksCorrectlyOverBoundary()
            {
                var bits = Enumerable.Repeat(
                        false,
                        65)
                    .ToArray();
                bits[0] = true;
                bits[64] = true;
                var longs = bits.ToLongs();
                Assert.Equal(2, longs.Length);
                Assert.Equal(1, longs[0]);
                Assert.Equal(1, longs[1]);
            }
        }

        public class BytesToLongsMethod
        {
            [Fact]
            public void Packs0x1Into1()
            {
                var bytes = new byte[] {0x1};
                var longs = bytes.ToLongs();
                Assert.Equal(1, longs.Length);
                Assert.Equal(1, longs[0]);
            }

            [Fact]
            public void Packs0x05FFInto1535()
            {
                var bytes = new byte[] { 0x05, 0xFF };
                var longs = bytes.ToLongs();
                Assert.Equal(1, longs.Length);
                Assert.Equal(1535, longs[0]);
            }

            [Fact]
            public void PacksCorrectlyOverBoundary()
            {
                var bytes = Enumerable.Repeat(
                        (byte) 0,
                        9)
                    .ToArray();
                bytes[0] = 1;
                bytes[8] = 1;
                var longs = bytes.ToLongs();
                Assert.Equal(2, longs.Length);
                Assert.Equal(1, longs[0]);
                Assert.Equal(1, longs[1]);
            }
        }
    }
}