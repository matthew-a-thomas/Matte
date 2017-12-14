using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    using System.Linq;
    using Matt.Lists;

    [TestClass]
    public class ReadOnlyListExtensionsClass
    {
        [TestClass]
        public class PickMethod
        {
            [TestMethod]
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
                Assert.IsTrue(picked.SequenceEqual(new [] { list[1] }));
            }
        }
    }
}