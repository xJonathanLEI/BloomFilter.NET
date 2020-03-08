using System;
using System.Collections.Generic;
using Xunit;

namespace BloomFilter.Tests
{
    public class BloomFilterTests
    {
        [Fact]
        public void Test1()
        {
            BloomFilter<string> bf = new BloomFilter<string>(20, 3);

            bf.Add("testing");
            bf.Add("nottesting");
            bf.Add("testingagain");

            Assert.False(bf.Contains("badstring"));
            Assert.True(bf.Contains("testing"));

            List<string> testItems = new List<string>() { "badstring", "testing", "test" };

            Assert.False(bf.ContainsAll(testItems));
            Assert.True(bf.ContainsAny(testItems));

            // False Positive Probability: 0.040894188143892
            Console.WriteLine("False Positive Probability: " + bf.FalsePositiveProbability());
        }
    }
}
