using Xunit;

namespace Matte.Random.Tests
{
    using System.Linq;
    using Moq;
    using Random;

    public class RandomExtensionsClass
    {
        public class ToEndlessBitSequenceMethod
        {
            [Fact]
            public void InvokesRandomPopulate()
            {
                var mocked = new Mock<IRandom>();
                mocked
                    .Object
                    .ToEndlessBitSequence()
                    .Take(10)
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    .ToList();
                mocked.Verify(x => x.Populate(It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()));
            }
        }
    }
}