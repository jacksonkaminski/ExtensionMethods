﻿// This project is licensed under The MIT License (MIT)
//
// Copyright (c) 2014 Jackson Kaminski
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy
//	of this software and associated documentation files (the "Software"), to deal
//	in the Software without restriction, including without limitation the rights
//	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//	copies of the Software, and to permit persons to whom the Software is
//	furnished to do so, subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in
//	all copies or substantial portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//	THE SOFTWARE.
//
// Please direct questions, patches, and suggestions to the project page at
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
        /// Get the array slice between the two indexes; If overflow or underflow occurs, 
        /// will return an empty array of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
        /// <param name="arr">Array to be sliced</param>
        /// <param name="start">Index at which the slice starts; cannot be negative.</param>
        /// <param name="end">Endpoint for the slice of the array. Supports negative values</param>
        /// <returns>Array slice adhering to the start and ending points, or a strongly
        /// typed empty array if an underflow or overflow situation is encountered</returns>
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
        /// Get the list slice between the two indexes; If overflow or underflow occurs, 
        /// will return an empty list of type T. Slice is inclusive for start, exclusive
        /// for end
        /// </summary>
        /// <param name="list">The list to be sliced</param>
        /// <param name="start">Index at which the slice starts; cannot be negative.</param>
        /// <param name="end">Endpoint for the slice of the list. Supports negative values</param>
        /// <returns>List slice adhering to the start and ending points, or a strongly
        /// typed empty array if an underflow or overflow situation is encountered</returns>
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
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
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
        /// <exception cref="InvalidOperationException">If Partition is called on an empty list when
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
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

        #region Init
        
        /// <summary>
        /// Takes a strongly typed array and returns a strongly typed array
        /// containing all but the last element in the original array; The 
        /// ordering of the contents of the array match that of the original
        /// array.
        /// </summary>
        /// <param name="arr">The array Init is called on</param>
        /// <returns>A strongly typed array containing all but the last element in the original array</returns>
        public static T[] Init<T>(this T[] arr)
        {
            return arr.Init<T>(false);
        }

        /// <summary>
        /// Takes a strongly typed array and returns a strongly typed array
        /// containing all but the last element in the original array; The 
        /// ordering of the contents of the array match that of the original
        /// array.
        /// </summary>
        /// <param name="arr">The array Init is called on</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if an exception should be raised if
        /// the array the method is invoked on is empty</param>
        /// <returns>A strongly typed array containing all but the last element in the original array</returns>
        /// <exception cref="InvalidOperationException">If Init is called on an empty array when
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
        public static T[] Init<T>(this T[] arr, bool throwErrorIfCollectionEmpty)
        {
            if (arr.Length == 0)
            {
                if (throwErrorIfCollectionEmpty)
                {
                    throw new InvalidOperationException("Init attempt failed due to empty collection");
                }

                return arr;
            }
            else
            {
                T[] newArr = new T[arr.Length - 1];
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    newArr[i] = arr[i];
                }

                return newArr;
            }
        }

        /// <summary>
        /// Takes a strongly typed list and returns a strongly typed list 
        /// containing all but the last element in the original list; The
        /// ordering of the contents of the list match that of the original list
        /// </summary>
        /// <param name="list">The list Init is called on</param>
        /// <returns>A strongly typed list containing all but the last element in the original list</returns>
        public static IList<T> Init<T>(this IList<T> list)
        {
            return list.Init<T>(false);
        }

        /// <summary>
        /// Takes a strongly typed list and returns a strongly typed list 
        /// containing all but the last element in the original list; The
        /// ordering of the contents of the list match that of the original list
        /// </summary>
        /// <param name="list">The list Init is called on</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if an exception should be raised if
        /// the list the method is invoked on is empty</param>
        /// <returns>A strongly typed list containing all but the last element in the original list</returns>
        /// <exception cref="InvalidOperationException">If Init is called on an empty list when
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
        public static IList<T> Init<T>(this IList<T> list, bool throwErrorIfCollectionEmpty)
        {
            if (list.Count == 0)
            {
                if (throwErrorIfCollectionEmpty)
                {
                    throw new InvalidOperationException("Init attempt failed due to empty collection");
                }

                return list;
            }
            else if (list.Count == 1)
            {
                return new List<T>();
            }
            else
            {
                IList<T> newList = new List<T>();
                for (int i = 0; i < list.Count - 1; i++)
                {
                    newList.Add(list[i]);
                }

                return newList;
            }
        }

        #endregion

        #region Tail

        /// <summary>
        /// Takes a strongly typed array and returns a strongly typed array
        /// containing all but the first element in the original array; The 
        /// ordering of the contents of the array match that of the original
        /// array.
        /// </summary>
        /// <param name="arr">The array Tail is called on</param>
        /// <returns>A strongly typed array containing all but the first element from the original array</returns>
        public static T[] Tail<T>(this T[] arr)
        {
            return arr.Tail<T>(false);
        }

        /// <summary>
        /// Takes a strongly typed array and returns a strongly typed array
        /// containing all but the first element in the original array; The 
        /// ordering of the contents of the array match that of the original
        /// array.
        /// </summary>
        /// <param name="arr">The array Tail is called on</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if an exception should be raised if
        /// the array the method is invoked on is empty</param>
        /// <returns>A strongly typed array containing all but the first element from the original array</returns>
        /// <exception cref="InvalidOperationException">If Tail is called on an empty array when
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
        public static T[] Tail<T>(this T[] arr, bool throwErrorIfCollectionEmpty)
        {
            if (arr.Length == 0)
            {
                if (throwErrorIfCollectionEmpty)
                {
                    throw new InvalidOperationException("Tail attempt failed due to empty collection");
                }

                return arr;
            }
            else
            {
                T[] newArr = new T[arr.Length - 1];
                for (int i = 1; i <= arr.Length - 1; i++)
                {
                    newArr[i-1] = arr[i];
                }

                return newArr;
            }
        }

        /// <summary>
        /// Takes a strongly typed list and returns a strongly typed list 
        /// containing all but the first element in the original list; The
        /// ordering of the contents of the list match that of the original list
        /// </summary>
        /// <param name="list">The list Tail is called on</param>
        /// <returns>A strongly typed list containing all but the first element in the original list</returns>
        public static IList<T> Tail<T>(this IList<T> list)
        {
            return list.Tail<T>(false);
        }

        /// <summary>
        /// Takes a strongly typed list and returns a strongly typed list 
        /// containing all but the first element in the original list; The
        /// ordering of the contents of the list match that of the original list
        /// </summary>
        /// <param name="list">The list Tail is called on</param>
        /// <param name="throwErrorIfCollectionEmpty">Determines if an exception should be raised if
        /// the list the method is invoked on is empty</param>
        /// <returns>A strongly typed list containing all but the first element in the original list</returns>
        /// <exception cref="InvalidOperationException">If Tail is called on an empty list when
        /// the throwErrorIfCollectionEmpty parameter is set to true, then this exception will be raised</exception>
        public static IList<T> Tail<T>(this IList<T> list, bool throwErrorIfCollectionEmpty)
        {
            if (list.Count == 0)
            {
                if (throwErrorIfCollectionEmpty)
                {
                    throw new InvalidOperationException("Tail attempt failed due to empty collection");
                }

                return list;
            }
            else if (list.Count == 1)
            {
                return new List<T>();
            }
            else
            {
                IList<T> newList = new List<T>();
                for (int i = 1; i < list.Count; i++)
                {
                    newList.Add(list[i]);
                }

                return newList;
            }
        }

        #endregion
    }
}
