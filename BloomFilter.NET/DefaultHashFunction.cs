using System;

namespace BloomFilter
{
    public class DefaultHashFunction<T> : HashFunction<T>
    {
        private readonly int bitSize;
        private readonly Random random;

        public DefaultHashFunction(T element, int bitSize)
        {
            this.bitSize = bitSize;
            this.random = new Random(element.GetHashCode());
        }

        public override int CalculateNextHash()
        {
            return random.Next(bitSize);
        }
    }

    public class DefaultHashFunctionFactory<T> : HashFunctionFactory<T>
    {
        public static DefaultHashFunctionFactory<T> Instance = new DefaultHashFunctionFactory<T>();

        public override HashFunction<T> Produce(T element, int bitSize)
        {
            return new DefaultHashFunction<T>(element, bitSize);
        }
    }
}