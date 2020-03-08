using System;
using System.Collections;
using System.Collections.Generic;

namespace BloomFilter
{
    public class BloomFilter<T>
    {
        public int BitSize { get; set; }
        public int SetSize { get; set; }
        public int NumberOfHashes { get; set; }

        private readonly BitArray bitArray;
        private readonly HashFunctionFactory<T> hashFuncFactory;

        /// <summary>
        /// Initializes the bloom filter and sets the optimal number of hashes. 
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        public BloomFilter(int bitSize, int setSize) : this(bitSize, setSize, DefaultHashFunctionFactory<T>.Instance) { }

        /// <summary>
        /// Initializes the bloom filter with a manual number of hashes.
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <param name="numberOfHashes">Number of hashing functions (k)</param>
        public BloomFilter(int bitSize, int setSize, int numberOfHashes) : this(bitSize, setSize, numberOfHashes, DefaultHashFunctionFactory<T>.Instance) { }

        /// <summary>
        /// Initializes the bloom filter and sets the optimal number of hashes. 
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <param name="hashFuncFactory">Hashing function factory</param>
        public BloomFilter(int bitSize, int setSize, HashFunctionFactory<T> hashFuncFactory) : this(bitSize, setSize, OptimalNumberOfHashes(bitSize, setSize), hashFuncFactory) { }

        /// <summary>
        /// Initializes the bloom filter with a manual number of hashes.
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <param name="numberOfHashes">Number of hashing functions (k)</param>
        /// <param name="hashFuncFactory">Hashing function factory</param>
        public BloomFilter(int bitSize, int setSize, int numberOfHashes, HashFunctionFactory<T> hashFuncFactory)
        {
            BitSize = bitSize;
            SetSize = setSize;
            NumberOfHashes = numberOfHashes;

            this.bitArray = new BitArray(bitSize);
            this.hashFuncFactory = hashFuncFactory;
        }

        /// <summary>
        /// Adds an item to the bloom filter.
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            var hashFunc = hashFuncFactory.Produce(item, BitSize);

            for (int i = 0; i < NumberOfHashes; i++)
                bitArray[hashFunc.CalculateNextHash()] = true;
        }

        /// <summary>
        /// Checks whether an item is probably in the set. False positives 
        /// are possible, but false negatives are not.
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the set probably contains the item</returns>
        public bool Contains(T item)
        {
            var hashFunc = hashFuncFactory.Produce(item, BitSize);

            for (int i = 0; i < NumberOfHashes; i++)
            {
                if (!bitArray[hashFunc.CalculateNextHash()])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Computes the probability of encountering a false positive.
        /// </summary>
        /// <returns>Probability of a false positive</returns>
        public double FalsePositiveProbability()
        {
            return Math.Pow((1 - Math.Exp(-NumberOfHashes * SetSize / (double)BitSize)), NumberOfHashes);
        }

        /// <summary>
        /// Hashing function for an object
        /// </summary>
        /// <param name="item">Any object</param>
        /// <returns>Hash of that object</returns>
        private int Hash(T item)
        {
            return item.GetHashCode();
        }

        /// <summary>
        /// Calculates the optimal number of hashes based on bloom filter
        /// bit size and set size.
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <returns>The optimal number of hashes</returns>
        private static int OptimalNumberOfHashes(int bitSize, int setSize)
        {
            return (int)Math.Ceiling((bitSize / setSize) * Math.Log(2.0));
        }
    }
}
