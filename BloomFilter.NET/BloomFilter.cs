﻿using System;
using System.Collections.Generic;
using System.Collections;

namespace BloomFilter
{
    public class BloomFilter<T>
    {
        Random _random;
        int _bitSize, _numberOfHashes, _setSize;
        BitArray _bitArray;

        #region Constructors
        /// <summary>
        /// Initializes the bloom filter and sets the optimal number of hashes. 
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        public BloomFilter(int bitSize, int setSize)
        {
            _bitSize = bitSize;
            _bitArray = new BitArray(bitSize);
            _setSize = setSize;
            _numberOfHashes = OptimalNumberOfHashes(_bitSize, _setSize);
        }

        /// <summary>
        /// Initializes the bloom filter with a manual number of hashes.
        /// </summary>
        /// <param name="bitSize">Size of the bloom filter in bits (m)</param>
        /// <param name="setSize">Size of the set (n)</param>
        /// <param name="numberOfHashes">Number of hashing functions (k)</param>
        public BloomFilter(int bitSize, int setSize, int numberOfHashes)
        {
            _bitSize = bitSize;
            _bitArray = new BitArray(bitSize);
            _setSize = setSize;
            _numberOfHashes = numberOfHashes;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Number of hashing functions (k)
        /// </summary>
        public int NumberOfHashes
        {
            set
            {
                _numberOfHashes = value;
            }
            get
            {
                return _numberOfHashes;
            }
        }

        /// <summary>
        /// Size of the set (n)
        /// </summary>
        public int SetSize
        {
            set
            {
                _setSize = value;
            }
            get
            {
                return _setSize;
            }
        }

        /// <summary>
        /// Size of the bloom filter in bits (m)
        /// </summary>
        public int BitSize
        {
            set
            {
                _bitSize = value;
            }
            get
            {
                return _bitSize;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds an item to the bloom filter.
        /// </summary>
        /// <param name="item">Item to be added</param>
        public void Add(T item)
        {
            _random = new Random(Hash(item));

            for (int i = 0; i < _numberOfHashes; i++)
                _bitArray[_random.Next(_bitSize)] = true;
        }

        /// <summary>
        /// Checks whether an item is probably in the set. False positives 
        /// are possible, but false negatives are not.
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the set probably contains the item</returns>
        public bool Contains(T item)
        {
            _random = new Random(Hash(item));

            for (int i = 0; i < _numberOfHashes; i++)
            {
                if (!_bitArray[_random.Next(_bitSize)])
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
            return Math.Pow((1 - Math.Exp(-_numberOfHashes * _setSize / (double)_bitSize)), _numberOfHashes);
        }
        #endregion

        #region Private Methods
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
        private int OptimalNumberOfHashes(int bitSize, int setSize)
        {
            return (int)Math.Ceiling((bitSize / setSize) * Math.Log(2.0));
        }
        #endregion
    }
}
