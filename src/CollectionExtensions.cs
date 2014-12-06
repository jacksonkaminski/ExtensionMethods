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
    public static class CollectionExtensions
    {
        #region Slice

        /// <summary>
        /// Get the array slice between the two indexes; If overflow or inderflow occurs, 
        /// will return an empty array of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
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
        /// Get the list slice between the two indexes; If overflow or inderflow occurs, 
        /// will return an empty list of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
        /// <param name="list">The list to be sliced</param>
        /// <param name="start">Index at which the slice starts; cannot be negative.</param>
        /// <param name="end">Endpoint for the slice of the list. Supports negative values</param>
        /// <returns>List slice adhereing to the start and ending points, or a strongly
        /// typed emtpy array if an underflow or overflow situation is encountered</returns>
        public static IList<T> Slice<T>(this IList<T> list, int start, int end)
        {
            int segmentEnd = end;
            //check for and handle underflow/overflow scenarios
            if (start < 0 || start > list.Count())
            {
                return new List<T>();
            }
            if (end > 0 && start > end || end == 0)
            {
                return new List<T>();
            }

            if (end < 0)
            {
                //end is negative value, so adding will result in subtraction
                segmentEnd = list.Count() + end;

                //handle scenario where negative end index causes underflow
                if (segmentEnd < start)
                {
                    return new List<T>();
                }
            }

            int len = segmentEnd - start;

            IList<T> newList = new List<T>();
            for (int i = start; i < start + len; i++)
            {
                newList.Add(list[i]);
            }

            return newList;
        }

        #endregion

        #region Partition

        /// <summary>
        /// Takes a list and splits it into two lists based on a predicate method supplied to Partition
        /// </summary>
        /// <param name="list">The list to be partitioned</param>
        /// <param name="partitionTest">The predicate method used to partition the list's contents</param>
        /// <returns>An Array containing two strongly-typed lists. The first list contains all elements
        /// that returned true when the predicate was applied; the second list contains the elements
        /// that returned false.</returns>
        public static IList<T>[] Partition<T>(this IList<T> list, Func<T, bool> partitionTest)
        {
            return list.Partition<T>(partitionTest, false);
        }

        /// <summary>
        /// Takes a list and splits it into two lists based on a predicate method supplied to Partition
        /// </summary>
        /// <param name="list">The list to be partitioned</param>
        /// <param name="partitionTest">The predicate method used to partition the list's contents</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if execution should halt if the list to be partitioned is empty</param>
        /// <returns>An Array containing two strongly-typed lists. The first list contains all elements
        /// that returned true when the predicate was applied; the second list contains the elements
        /// that returned false.</returns>
        /// <exception cref="InvalidOperationException">If Partition is called on an empty list when
        /// the OnEmptyCollection.ThrowException option is selected, then this exception will be raised</exception>
        public static IList<T>[] Partition<T>(this IList<T> list, Func<T, bool> partitionTest, bool throwErrorIfCollectionEmpty)
        {
            IList<T> passTest = new List<T>();
            IList<T> failTest = new List<T>();

            if (list.Count == 0)
            {
                if (throwErrorIfCollectionEmpty == false)
                {
                    return new IList<T>[] { passTest, failTest };
                }

                throw new InvalidOperationException("Partition attempt failed due to empty collection");
            }

            foreach (var item in list)
            {
                if (partitionTest(item))
                {
                    passTest.Add(item);
                }
                else
                {
                    failTest.Add(item);
                }
            }

            return new IList<T>[] { passTest, failTest };
        }

        /// <summary>
        /// Takes an array and splits it into two lists based on a predicate method supplied to Partition
        /// </summary>
        /// <param name="array">The array to be partitioned</param>
        /// <param name="partitionTest">The predicate method used to partition the array's contents</param>
        /// <returns>An Array containing two arrays. The first array contains all elements
        /// that returned true when the predicate was applied; the second array contains the elements
        /// that returned false.</returns>
        public static T[][] Partition<T>(this T[] array, Func<T, bool> partitionTest)
        {
            return array.Partition<T>(partitionTest, false);
        }

        /// <summary>
        /// Takes an array and splits it into two lists based on a predicate method supplied to Partition
        /// </summary>
        /// <param name="array">The array to be partitioned</param>
        /// <param name="partitionTest">The predicate method used to partition the array's contents</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if execution should halt if the array to be partitioned is empty</param>
        /// <returns>An Array containing two arrays. The first array contains all elements
        /// that returned true when the predicate was applied; the second array contains the elements
        /// that returned false.</returns>
        /// <exception cref="InvalidOperationException">If Partition is called on an empty array when
        /// the OnEmptyCollection.ThrowException option is selected, then this exception will be raised</exception>
        public static T[][] Partition<T>(this T[] array, Func<T, bool> partitionTest, bool throwErrorIfCollectionEmpty)
        {
            IList<T> passTest = new List<T>();
            IList<T> failTest = new List<T>();

            if (array.Length == 0)
            {
                if (throwErrorIfCollectionEmpty == false)
                {
                    return new T[2][] { new T[0], new T[0] };
                }

                throw new InvalidOperationException("Partition attempt failed due to empty collection");
            }

            foreach (var item in array)
            {
                if (partitionTest(item))
                {
                    passTest.Add(item);
                }
                else
                {
                    failTest.Add(item);
                }
            }

            T[][] results = new T[2][] { passTest.ToArray(), failTest.ToArray() }; 

            return results;
        }

        #endregion

    }
}
