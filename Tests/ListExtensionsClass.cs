namespace Tests
{
    using System.Linq;
    using Matt.Lists;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListExtensionsClass
    {
        [TestClass]
        public class CombineIntoMethod
        {
            [TestMethod]
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
                Assert.IsTrue(into.SequenceEqual(new byte[] { 0x11, 0x11, 0x11 }));
            }
        }
    }
}