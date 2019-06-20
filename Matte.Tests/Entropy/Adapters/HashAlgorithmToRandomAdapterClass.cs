namespace Matte.Tests.Entropy.Adapters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using Matte.Entropy.Adapters;
    using Moq;
    using Xunit;

    public class HashAlgorithmToRandomAdapterClass
    {
        public class BufferPropertyShould
        {
            [Fact]
            public void RetrieveHashAlgorithmBuffer()
            {
                var hashAlgorithm = new Mock<HashAlgorithm>();
                var adapter = new HashAlgorithmToRandomAdapter(hashAlgorithm.Object, 0);

                GC.KeepAlive(adapter.Buffer);
                
                hashAlgorithm.VerifyGet(x => x.Hash);
            }
        }
        
        public class DisposeMethodShould
        {
            class MockHashAlgorithm : HashAlgorithm
            {
                public int DisposalCount;
                
                protected override void Dispose(bool disposing)
                {
                    Interlocked.Increment(ref DisposalCount);
                    base.Dispose(disposing);
                }

                protected override void HashCore(byte[] array, int ibStart, int cbSize) {}

                protected override byte[] HashFinal() => new byte[0];

                public override void Initialize() {}
            }

            [Fact]
            public void DisposeOfInjectedHashAlgorithm()
            {
                var hashAlgorithm = new MockHashAlgorithm();
                var adapter = new HashAlgorithmToRandomAdapter(
                    hashAlgorithm,
                    0);
                
                adapter.Dispose();
                
                Assert.Equal(1, hashAlgorithm.DisposalCount);
            }
        }

        public class PopulateMethodShould
        {
            class MockHashAlgorithm : HashAlgorithm
            {
                public List<byte[]> HashRequests { get; } = new List<byte[]>();
                
                protected override void HashCore(byte[] array, int ibStart, int cbSize)
                {
                    var clone = array.Clone() as byte[];
                    HashRequests.Add(clone);
                }

                protected override byte[] HashFinal() => new byte[0];

                public override void Initialize() {}
            }
            
            [Fact]
            public void ComputeHashOfIncreasingSeedValues()
            {
                var hashAlgorithm = new MockHashAlgorithm();
                var adapter = new HashAlgorithmToRandomAdapter(
                    hashAlgorithm,
                    0);
                
                adapter.Populate();
                adapter.Populate();
                adapter.Populate();

                var hashedValues = hashAlgorithm
                    .HashRequests
                    .Select(x => BitConverter.ToInt64(x));
                Assert.Equal(new long[] {1, 2, 3}, hashedValues);
            }
        }

        [Fact]
        public void ShouldWorkWithSha256()
        {
            using (var adapter = new HashAlgorithmToRandomAdapter(
                SHA256.Create(),
                0))
            {
                adapter.Populate();
                var buffer = adapter.Buffer.Clone() as byte[];

                using (var sha256 = SHA256.Create())
                {
                    var computedHash = sha256.ComputeHash(BitConverter.GetBytes(1L));
                    
                    Assert.Equal(computedHash, buffer);
                }
            }
        }
    }
}