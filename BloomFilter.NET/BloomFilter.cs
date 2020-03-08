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

        /// <summary>
        /// Initializes the bloom filter and sets the optimal number of hashes. 
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        public BloomFilter(int bitSize, int setSize) : this(bitSize, setSize, OptimalNumberOfHashes(bitSize, setSize)) { }

        /// <summary>
        /// Initializes the bloom filter with a manual number of hashes.
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <param name="numberOfHashes">Number of hashing functions (k)</param>
        public BloomFilter(int bitSize, int setSize, int numberOfHashes)
        {
            BitSize = bitSize;
            SetSize = setSize;
            NumberOfHashes = numberOfHashes;

            bitArray = new BitArray(bitSize);
        }

        /// <summary>
        /// Adds an item to the bloom filter.
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            var random = new Random(Hash(item));

            for (int i = 0; i < NumberOfHashes; i++)
                bitArray[random.Next(BitSize)] = true;
        }

        /// <summary>
        /// Checks whether an item is probably in the set. False positives 
        /// are possible, but false negatives are not.
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the set probably contains the item</returns>
        public bool Contains(T item)
        {
            var random = new Random(Hash(item));

            for (int i = 0; i < NumberOfHashes; i++)
            {
                if (!bitArray[random.Next(BitSize)])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if any item in the list is probably in the set.
        /// </summary>
        /// <param name="items">List of items to be checked</param>
        /// <returns>True if the bloom filter contains any of the items in the list</returns>
        public bool ContainsAny(List<T> items)
        {
            foreach (T item in items)
            {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if all items in the list are probably in the set.
        /// </summary>
        /// <param name="items">List of items to be checked</param>
        /// <returns>True if the bloom filter contains all of the items in the list</returns>
        public bool ContainsAll(List<T> items)
        {
            foreach (T item in items)
            {
                if (!Contains(item))
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
