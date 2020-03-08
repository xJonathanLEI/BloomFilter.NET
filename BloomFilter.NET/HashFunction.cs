namespace BloomFilter
{
    public abstract class HashFunction<T>
    {
        public abstract int CalculateNextHash();
    }

    public abstract class HashFunctionFactory<T>
    {
        public abstract HashFunction<T> Produce(T element, int bitSize);
    }
}