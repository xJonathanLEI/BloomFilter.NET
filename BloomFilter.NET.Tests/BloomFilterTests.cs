using Xunit;

namespace BloomFilter.Tests
{
    public class BloomFilterTests
    {
        [Fact]
        public void ContainsTest()
        {
            BloomFilter<string> bf = new BloomFilter<string>(20, 3);

            bf.Add("testing");
            bf.Add("nottesting");
            bf.Add("testingagain");

            Assert.False(bf.Contains("badstring"));
            Assert.True(bf.Contains("testing"));
            Assert.True(bf.Contains("nottesting"));
            Assert.True(bf.Contains("testingagain"));
        }
    }
}
