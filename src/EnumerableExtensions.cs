// This project is licensed under The MIT License (MIT)
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

namespace EnumerationExtensions
{
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
            return source.Partition<T>(partitionTest, false);
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
        /// <param name="throwErrorIfCollectionEmpty">The strategy to apply if the collection being operated
        /// on is empty; Options are to return an empty collection or throw an Exception</param>
        /// /// <returns>An Array containing two enumerable collections. The first list contains all elements
        /// that returned true when the predicate was applied; the second list contains the elements
        /// that returned false.</returns>
        /// <exception cref="InvalidOperationException">If Partition is called on an empty collection when
        /// the OnEmptyCollection.ThrowException option is selected, then this exception will be raised</exception>
        public static IEnumerable<T>[] Partition<T>(this IEnumerable<T> source, Func<T, bool> partitionTest, bool throwErrorIfCollectionEmpty)
        {
            IEnumerable<T> passTest = Enumerable.Empty<T>();
            IEnumerable<T> failTest = Enumerable.Empty<T>();

            if (source.Count() == 0)
            {
                if (throwErrorIfCollectionEmpty == false)
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

        /// <summary>
        /// Takes an enumberable collection and splits it into a series of lists of a specified size
        /// </summary>
        /// <typeparam name="T">The enumerable's type</typeparam>
        /// <param name="source">The enumerable to be chunked</param>
        /// <param name="numOfElements">The size of each chunk (Must be 1 or greater)</param>
        /// <returns>An Enumerable containing the contents of the original collection,
        /// split into a series of lists of size 'numOfElements'</returns>
        /// <exception cref="InvalidOperationException">Thrown if the numOfElements value 
        /// is less than 1; Also thrown if throwErrorIfCollectionEmpty is set to true and
        /// the collection Chunk is called on is empty</exception>
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int numOfElements)
        {
            return source.Chunk<T>(numOfElements, false);
        }

        /// <summary>
        /// Takes an enumberable collection and splits it into a series of lists of a specified size
        /// </summary>
        /// <typeparam name="T">The enumerable's type</typeparam>
        /// <param name="source">The enumerable to be chunked</param>
        /// <param name="numOfElements">The size of each chunk (Must be 1 or greater)</param>
        /// <param name="throwErrorIfCollectionEmpty">The strategy to apply if the collection being operated
        /// on is empty; Options are to return an empty collection or throw an Exception</param>
        /// <returns>An Enumerable containing the contents of the original collection,
        /// split into a series of lists of size 'numOfElements'</returns>
        /// <exception cref="InvalidOperationException">Thrown if the numOfElements value 
        /// is less than 1; Also thrown if throwErrorIfCollectionEmpty is set to true and
        /// the collection Chunk is called on is empty</exception>
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int numOfElements, bool throwErrorIfCollectionEmpty)
        {
            if (source.Count() == 0 && throwErrorIfCollectionEmpty)
            {
                throw new InvalidOperationException("Chunk attempt failed due to empty collection");
            }

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

        #region JaccardIndexSort

        /// <summary>
        /// Takes a primary collection and a set of other collections of the same type, and 
        /// sorts them in order of their similarity to the primary collection by calculating
        /// the set of collections' Jaccard similarity coefficients
        /// </summary>
        /// <param name="source">The enumerable collection being evaluated</param>
        /// <param name="compareTo">The set of enumerable collections to sort based on their similarity to the source collection</param>
        /// <exception cref="ArgumentNullException">Thrown if either source and compareTo are null</exception>
        /// <exception cref="InvalidOperationException">Thrown if either source or compareTo are empty collections</exception>
        /// <remarks>
        /// This method is meant to be used to calculate Jaccard coefficients against non-binary datasets
        /// For more information about the Jaccard Index (Jaccard similarity coefficient) see the 
        /// following pages:
        /// http://people.revoledu.com/kardi/tutorial/Similarity/Jaccard.html
        /// http://en.wikipedia.org/wiki/Jaccard_index
        /// </remarks>
        public static IEnumerable<IEnumerable<T>> JaccardIndexSort<T>(this IEnumerable<T> source, IEnumerable<IEnumerable<T>> compareTo)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (compareTo == null) throw new ArgumentNullException("compareTo");

            if (source.Count() == 0) throw new InvalidOperationException("JaccardSort cannot operate on empty source");
            if (compareTo.Count() == 0) throw new InvalidOperationException("JaccardSort cannot operate against empty compare collection");

            IList<Tuple<double, int>> jaccardValues = new List<Tuple<double, int>>();

            for (int i = 0; i < compareTo.Count(); i++)
            {
                double calcJaccardIndex = CalculateJaccardIndex<T>(source, compareTo.ElementAt(i));
                jaccardValues.Add(new Tuple<double, int>(calcJaccardIndex, i));
            }

            foreach (var item in jaccardValues.OrderByDescending(x => x.Item1))
            {
                yield return compareTo.ElementAt(item.Item2);
            }
        }

        /// <summary>
        /// Compares to Enumerable collections determining the Jaccard similarity coefficient 
        /// of the compareTo collection as it relates to the source collection
        /// </summary>
        /// <param name="source">The primary collection</param>
        /// <param name="compareTo">The collection to compare to source</param>
        /// <returns>The list of collections, sorted by their Jaccard Simimlarity Index, sorted more
        /// similar to less similar</returns>
        /// <exception cref="ArgumentNullException">Thrown if either source and compareTo are null</exception>
        /// <exception cref="InvalidOperationException">Thrown if either source or compareTo are empty collections</exception>
        /// <remarks>
        /// This method is meant to be used to calculate Jaccard coefficients against non-binary datasets
        /// For more information about the Jaccard Index (Jaccard similarity coefficient) see the 
        /// following pages:
        /// http://people.revoledu.com/kardi/tutorial/Similarity/Jaccard.html
        /// http://en.wikipedia.org/wiki/Jaccard_index
        /// </remarks>
        public static double GetJaccardIndex<T>(this IEnumerable<T> source, IEnumerable<T> compareTo)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (compareTo == null) throw new ArgumentNullException("compareTo");

            if (source.Count() == 0) throw new InvalidOperationException("JaccardSort cannot operate on empty source");
            if (compareTo.Count() == 0) throw new InvalidOperationException("JaccardSort cannot operate against empty compare collection");

            return CalculateJaccardIndex<T>(source, compareTo);
        }

        /// <summary>
        /// Compares an object of type T against the set of comparators and returns
        /// the Jaccard similarity coefficient of the object relative to the rules
        /// as specififed in the comparators set
        /// </summary>
        /// <param name="source">The object to be tested for similarity</param>
        /// <param name="comparators">A series of predicate methods used to perfom the object comparison</param>
        /// <returns>A value between 1.0 and 0.0 denoting the similarity coefficient</returns>
        /// <exception cref="ArgumentNullException">Thrown if either source and comparators list are null</exception>
        /// <exception cref="InvalidOperationException">Thrown if the comparators list is empty</exception>
        /// <remarks>
        /// This method is meant to be used to calculate binary variant of Jaccard coefficient 
        /// calculation, and uses the comparators predicates as the representation of
        /// (a proxy for) the object being compared against to determine the similarity coefficient.
        /// For more information about the Jaccard Index (Jaccard similarity coefficient) see the 
        /// following pages:
        /// http://people.revoledu.com/kardi/tutorial/Similarity/Jaccard.html
        /// http://en.wikipedia.org/wiki/Jaccard_index
        /// </remarks>
        public static double GetJaccardIndex<T>(this T source, IList<Func<T, bool>> comparators)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (comparators == null) throw new ArgumentNullException("comparators");

            if (comparators.Count() == 0) throw new InvalidOperationException("Comparators list must contain comparators");

            return CalculateJaccardIndex<T>(source, comparators);
        }

        /// <summary>
        /// Takes a list of objects and sorts them in order of similarity using the Binary 
        /// variant of the Jaccard similarity coefficient
        /// </summary>
        /// <param name="source">The list objects to be tested for similarity</param>
        /// <param name="comparators">A series of predicate methods used to perfom the object comparison</param>
        /// <returns>The list of objects, sorted by their Jaccard Simimlarity Index, sorted more
        /// similar to less similar</returns>
        /// <exception cref="ArgumentNullException">Thrown if either source and comparators list are null</exception>
        /// <exception cref="InvalidOperationException">Thrown if the comparators list is empty</exception>
        /// <remarks>
        /// This method is meant to be used to calculate binary variant of Jaccard coefficient calculation
        /// when performing the sort operation, and uses the comparators predicates as the representation of
        /// (a proxy for) the object being compared against to determine sort order.
        /// For more information about the Jaccard Index (Jaccard similarity coefficient) see the 
        /// following pages:
        /// http://people.revoledu.com/kardi/tutorial/Similarity/Jaccard.html
        /// http://en.wikipedia.org/wiki/Jaccard_index
        /// </remarks> 
        public static IEnumerable<T> JaccardIndexSort<T>(this IEnumerable<T> source, IList<Func<T, bool>> comparators)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (comparators == null) throw new ArgumentNullException("comparators");

            if (comparators.Count() == 0) throw new InvalidOperationException("Comparators list must contain comparators");

            IList<Tuple<double, int>> jaccardValues = new List<Tuple<double, int>>();

            for (int i = 0; i < source.Count(); i++)
            {
                double calcJaccardIndex = CalculateJaccardIndex<T>(source.ElementAt(i), comparators);
                jaccardValues.Add(new Tuple<double, int>(calcJaccardIndex, i));
            }

            foreach (var item in jaccardValues.OrderByDescending(x => x.Item1))
            {
                yield return source.ElementAt(item.Item2);
            }
        }

        //Performs the actual calculation to derive the Jaccard Index
        private static double CalculateJaccardIndex<T>(IEnumerable<T> source, IEnumerable<T> compareTo)
        {
            if (source.Count() == 0 && compareTo.Count() == 0) return 1.0;

            IEnumerable<T> intersection = source.Intersect<T>(compareTo);
            IEnumerable<T> union = source.Union<T>(compareTo);

            return intersection.Count() / (double)union.Count();
        }

        //Performs the actual calculation to derive the Jaccard Index
        //Note that the comparison object to whome the source's 'similarity' is
        //being calculated is a fictional object who, when subjected to testing
        //using the comparators list of predicates returns all true values
        private static double CalculateJaccardIndex<T>(T source, IList<Func<T, bool>> comparators)
        {
            int bothTrue = 0;

            //In the below loop, there is the second collection that is visibily unrepresented,
            //but is the set of always 'true' values that would be returned by the set of all
            //comparator functions; And since this version of the coefficent's calculation is 
            // [both true / (both true + first side true + second side true)]
            //where one side is always true, we can restate the calculation as
            // [both true / count of comparators] 
            //as all binary compare pairs resulting will be either 'both true' or 'first side true'
            //and the sum of those will be the count of comparators
            foreach (var returnValue in comparators.Select(f => f(source)))
            {
                if (returnValue == true)
                {
                    bothTrue++;
                }
            }

            return (double)bothTrue / comparators.Count();
        }

        #endregion
    }
}
