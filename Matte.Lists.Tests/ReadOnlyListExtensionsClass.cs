namespace Matte.Lists.Tests
{
    using System.Linq;
    using Xunit;

    public class ReadOnlyListExtensionsClass
    {
        public class PickMethod
        {
            [Fact]
            public void PicksOneOutOfAList()
            {
                var list = new byte[]
                {
                    0x00,
                    0x01,
                    0x02,
                    0x03
                };
                var picked = list.Pick(new [] { false, true }).ToList();
                Assert.True(picked.SequenceEqual(new [] { list[1] }));
            }
        }
    }
}