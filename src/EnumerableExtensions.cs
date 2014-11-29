// This project is licensed under The MIT License (MIT)
//
// Copyright 2014 Jackson Kaminski
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//		of this software and associated documentation files (the "Software"), to deal
//		in the Software without restriction, including without limitation the rights
//		to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//		copies of the Software, and to permit persons to whom the Software is
//		furnished to do so, subject to the following conditions:
//
//		The above copyright notice and this permission notice shall be included in
//		all copies or substantial portions of the Software.
//
//		THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//		IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//		FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//		AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//		LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//		OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//		THE SOFTWARE.
//
// Please direct questions, patches, and suggestions to the project page at

// @TODO - FINISH PATH
// https://github.com/...

using System;
using System.Collections.Generic;
using System.Linq;

namespace EnumerationExtensions
{
    public enum OnEmptyCollection
    {
        ReturnEmptyCollections = 0,
        ThrowException
    }

    public static class EnumerableCollectionExtensions
    {

        #region Partition

        /// <summary>
        /// Takes a collection and a predicate function, and returns an array that contains the contents
        /// of the original collection split into 2 collections. The first collection contains all items
        /// that returned true from the partition test, the second collection contains all the items that
        /// returned false from the partition test
        /// </summary>
        /// <typeparam name="T">The type for the collection the partition is to be applied to</typeparam>
        /// <param name="source">The collection to be partitioned</param>
        /// <param name="partitionTest">The predicate used to partition the collection's contents</param>
        public static IEnumerable<T>[] Partition<T>(this IEnumerable<T> source, Func<T, bool> partitionTest)
        {
            return source.Partition(partitionTest, OnEmptyCollection.ReturnEmptyCollections);
        }

        /// <summary>
        ///  Takes a collection and a predicate function, and returns an array that contains the contents
        /// of the original collection split into 2 collections. The first collection contains all items
        /// that returned true from the partition test, the second collection contains all the items that
        /// returned false from the partition test
        /// </summary>
        /// <typeparam name="T">The type for the collection the partition is to be applied to</typeparam>
        /// <param name="source">The collection to be partitioned</param>
        /// <param name="partitionTest">The predicate method used to partition the collection's contents</param>
        /// <param name="emptyCollectionStrategy">The strategy to apply if the collection being operated
        /// on is empty; Options are to return an empty collection or throw an Exception</param>
        /// /// <returns>An Array containing two enumerable collections. The first list contains all elements
        /// that returned true when the predicate was applied; the second list contains the elements
        /// that returned false.</returns>
        /// <exception cref="InvalidOperationException">If Partition is called on an empty collection when
        /// the OnEmptyCollection.ThrowException option is selected, then this exception will be raised</exception>
        public static IEnumerable<T>[] Partition<T>(this IEnumerable<T> source, Func<T, bool> partitionTest, OnEmptyCollection emptyCollectionStrategy)
        {
            IEnumerable<T> passTest = Enumerable.Empty<T>();
            IEnumerable<T> failTest = Enumerable.Empty<T>();

            if (source.Count() == 0)
            {
                if (emptyCollectionStrategy == OnEmptyCollection.ReturnEmptyCollections)
                {
                    return new IEnumerable<T>[] { passTest, failTest }; 
                }

                throw new InvalidOperationException("Partition attempt failed due to empty collection");
            }

            foreach (var item in source)
            {
                if (partitionTest(item))
                {
                    passTest = passTest.Append<T>(item);
                }
                else
                {
                    failTest = failTest.Append<T>(item);
                }
            }

            return new IEnumerable<T>[] { passTest, failTest };
        }

        /// <summary>
        /// Appends a value to an IEnumerable type collection
        /// </summary>
        /// <typeparam name="T">The type for the collection</typeparam>
        /// <param name="enumerable">The collection</param>
        /// <param name="value">The Value to be appended to the end of the collection</param>
        private static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach (var cur in enumerable)
            {
                yield return cur;
            }
            yield return value;
        }

        #endregion

        #region Chunk

        // @TODO - NEED TO ADD TESTS
        /// <summary>
        /// Takes an enumberable collection and splits it into a set of lists of a specific size
        /// </summary>
        /// <typeparam name="T">The enumerable's type</typeparam>
        /// <param name="source">The enumerable to be chunked</param>
        /// <param name="numOfElements">The size of each chunk (Must be 1 or greater)</param>
        /// <returns>An Enumerable containing the contents of the original collection,
        /// split into a series of lists of size 'numOfElements'</returns>
        /// <exception cref="InvalidOperationException">Thrown if the numOfElements value 
        /// is less than 1</exception>
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int numOfElements)
        {
            if (numOfElements <= 0)
            {
                throw new InvalidOperationException("The Chunk size must be 1 or greater");
            }

            IList<T> list = new List<T>();

            foreach (var item in source)
            {
                list.Add(item);

                if (list.Count() % numOfElements == 0)
                {
                    yield return list;
                    list = new List<T>();
                }
            }

            if (source.Count() % numOfElements != 0)
            {
                //handles case where last list might be less than numOfElements size
                yield return list;
            }
        }

        #endregion
    }
}
