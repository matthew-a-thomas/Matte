namespace Tests
{
    using System.Linq;
    using Matt.Bits;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FastArrayClass
    {
        [TestMethod]
        public void GetSameBytesOutAsWerePutIn()
        {
            var original = new byte[]
            {
                0x01,
                0x02,
                0x03,
                0x04,
                0x05
            };
            var array = FastArray.FromBytes(original);
            var bytes = array.ToBytes();
            Assert.IsNotNull(bytes);
            Assert.IsTrue(original.SequenceEqual(bytes));
        }

        [TestClass]
        public class XorMethod
        {
            [TestMethod]
            public void CorrectlyPerformsXor()
            {
                var original = new byte[]
                {
                    0x01,
                    0x02,
                    0x03,
                    0x04,
                    0x05,
                    0x06,
                    0x07,
                    0x08
                };
                var delta = new byte[]
                {
                    0x82,
                    0x51
                };
                var expected =
                    original
                        .Select((x, i) => i < delta.Length ? (byte)(x ^ delta[i]) : x)
                        .ToArray();

                var a = FastArray.FromBytes(original);
                var b = FastArray.FromBytes(delta);
                a.Xor(b);
                var result = a.ToBytes();
                Assert.IsTrue(expected.SequenceEqual(result));
            }
        }
    }
}