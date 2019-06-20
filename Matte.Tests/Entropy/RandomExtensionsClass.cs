namespace Matte.Tests.Entropy
{
    using System.Linq;
    using Matte.Entropy;
    using Moq;
    using Xunit;

    public class RandomExtensionsClass
    {
        public class ToEndlessBitSequenceMethod
        {
            [Fact]
            public void InvokesRandomPopulate()
            {
                var mocked = new Mock<IRandom>();
                mocked.SetupGet(x => x.Buffer)
                    .Returns(new byte[] {0});
                mocked
                    .Object
                    .ToEndlessBitSequence()
                    .Take(10)
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    .ToList();
                mocked.Verify(x => x.Populate());
            }
        }
    }
}