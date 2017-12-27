namespace Matte.Lists.Tests
{
    using System.Linq;
    using Xunit;

    public class ListExtensionsClass
    {
        public class CombineIntoMethod
        {
            [Fact]
            public void Combines()
            {
                var from = new byte[]
                {
                    0x00,
                    0x01,
                    0x10
                };
                var into = new byte[]
                {
                    0x11,
                    0x10,
                    0x01
                };
                from.CombineInto(into, (x, y) => (byte)(x ^ y));
                Assert.True(into.SequenceEqual(new byte[] { 0x11, 0x11, 0x11 }));
            }
        }
    }
}