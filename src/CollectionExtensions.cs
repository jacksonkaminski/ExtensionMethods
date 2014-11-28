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

namespace CollectionExtensions
{
    public enum OnEmptyCollection
    {
        ReturnEmptyCollections = 0,
        ThrowException
    }

    public static class CollectionPartitionExtension
    {
        
        /// <summary>
        /// Get the array slice between the two indexes; If overflow or inderflow occurs, 
        /// will return an empty array of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="arr">Array to be sliced</param>
        /// <param name="start">Index at which the slice starts; cannot be negative.</param>
        /// <param name="end">Endpoint for the slice of the array. Supports negative values</param>
        /// <returns>Array slice adhereing to the start and ending points, or a strongly
        /// typed emtpy array if an underflow or overflow situation is encountered</returns>
        public static T[] Slice<T>(this T[] arr, int start, int end)
        {
            int segmentEnd = end;
            //check for and handle underflow/overflow scenarios
            if (start < 0 || start > arr.Length)
            {
                return new T[0];
            }
            if (end > 0 && start > end || end == 0)
            {
                return new T[0];
            }

            if (end < 0)
            {
                //end is negative value, so adding will result in subtraction
                segmentEnd = arr.Length + end;

                //handle scenario where negative end index causes underflow
                if (segmentEnd < start)
                {
                    return new T[0];
                }
            }

            int len = segmentEnd - start;

            T[] newArry = new T[len];
            for (int i = 0; i < len; i++)
            {
                newArry[i] = arr[i + start];
            }

            return newArry;
        }

        /// <summary>
        /// Takes a collection and a predicate function, and returns an array that contains the contents
        /// of the original collection split into 2 collections. The first collection contains all items
        /// that returned true from the partition test, the second collection contains all the items that
        /// returned false from the partition test
        /// </summary>
        /// <typeparam name="T">The type for the collection the partition is to be applied to</typeparam>
        /// <param name="collection">The collection to be partitioned</param>
        /// <param name="partitionTest">The predicate used to partition the collection's contents</param>
        public static IEnumerable<T>[] Partition<T>(this IEnumerable<T> collection, Func<T, bool> partitionTest)
        {
            return collection.Partition(partitionTest, OnEmptyCollection.ReturnEmptyCollections);
        }

        /// <summary>
        ///  Takes a collection and a predicate function, and returns an array that contains the contents
        /// of the original collection split into 2 collections. The first collection contains all items
        /// that returned true from the partition test, the second collection contains all the items that
        /// returned false from the partition test
        /// </summary>
        /// <typeparam name="T">The type for the collection the partition is to be applied to</typeparam>
        /// <param name="collection">The collection to be partitioned</param>
        /// <param name="partitionTest">The predicate used to partition the collection's contents</param>
        /// <param name="emptyCollectionStrategy">The strategy to apply if the collection being operated
        /// on is empty; Options are to return an empty collection or throw an Exception</param>
        /// <exception cref="InvalidOperationException">If Partition is called on an empty collection when
        /// the OnEmptyCollection.ThrowException option is selected, then this exception will be raised</exception>
        public static IEnumerable<T>[] Partition<T>(this IEnumerable<T> collection, Func<T, bool> partitionTest, OnEmptyCollection emptyCollectionStrategy)
        {
            IEnumerable<T> passTest = Enumerable.Empty<T>();
            IEnumerable<T> failTest = Enumerable.Empty<T>();

            if (collection.Count() == 0)
            {
                if (emptyCollectionStrategy == OnEmptyCollection.ReturnEmptyCollections)
                {
                    return new IEnumerable<T>[] { passTest, failTest }; 
                }

                throw new InvalidOperationException("Partition attempt failed due to empty collection");
            }

            foreach (var item in collection)
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
        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T value)
        {
            foreach (var cur in enumerable)
            {
                yield return cur;
            }
            yield return value;
        }
    }
}
