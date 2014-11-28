﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using CollectionExtensions;

namespace ExtensionMethodTests
{
    [TestFixture]
    public class TestsCollectionPartitionMethods
    {
        int[] _array;
        int _size = 10;
        List<Person> _persons;

        [SetUp]
        public void SetUp()
        {
            _array = new int[] { 0, 1, 2, 3 ,4 ,5 ,6 ,7 ,8 ,9 };
            _persons = GetPersonList();
        }

        #region Tests for Slice()

        [Test]
        public void SliceCanHandleUnderflow()
        {
            Assert.AreEqual(new int[0], _array.Slice(-1, 5)); //start underflow
            Assert.AreEqual(new int[0], _array.Slice(1, -20)); //underflow
            Assert.AreEqual(new int[0], _array.Slice(0, _size * -1)); //underflow boundry check
            Assert.AreEqual(new int[0], _array.Slice(1, _size * -1)); //underflow due to position of starting point
        }

        [Test]
        public void SliceCanHandleOverflow()
        {
            Assert.AreEqual(new int[0], _array.Slice(_size + 1, 5));
        }

        [Test]
        public void CanSlice()
        {
            Assert.AreEqual(_array.Length, 10);
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, _array.Slice(0, _size)); //boundary check
            Assert.AreEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, _array.Slice<int>(0, -1)); //boundary and negative end check
            Assert.AreEqual(new int[] { 2, 3, 4 }, _array.Slice<int>(2, -5)); //negative end check
            Assert.AreEqual(new int[] { 5, 6, 7, 8 }, _array.Slice<int>(5, 9));
        }

        #endregion

        #region Tests for Partition

        [Test]
        public void CanPartitionPrimitive()
        {
            IEnumerable<int>[] partitionedResults1 = _array.Partition<int>(x => x > 5);
            Assert.AreEqual(partitionedResults1.Length, 2);
            Assert.AreEqual(partitionedResults1[0].Count(), 4);
            Assert.AreEqual(partitionedResults1[1].Count(), 6);
        }

        [Test]
        public void CanPartitionComplexType()
        {
            IEnumerable<Person>[] partitionedResults = _persons.Partition<Person>(x => x.Gender == 'M');
            Assert.AreEqual(partitionedResults.Length, 2);
            Assert.AreEqual(partitionedResults[0].Count(), 4);
            Assert.AreEqual(partitionedResults[1].Count(), 3);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanThrowExceptionOEmptyCollection()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            IEnumerable<Person>[] partitionedResults = 
                        noPersons.Partition<Person>(x => x.Gender == 'M', OnEmptyCollection.ThrowException);

        }

        [Test]
        public void CanReturnEmptyCollectionIfEmptyCollection()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            IEnumerable<Person>[] partitionedResults =
                        noPersons.Partition<Person>(x => x.Gender == 'M', OnEmptyCollection.ReturnEmptyCollections);

            Assert.IsTrue(partitionedResults[0].Count() == 0);
            Assert.IsTrue(partitionedResults[1].Count() == 0);
        }

        #endregion

        #region nested class

        class Person
        {
            internal string FirstName { get; set; }
            internal string Lastname { get; set; }
            internal char Gender { get; set; }
            internal int Age { get; set; }
        }

        #endregion

        #region private methods

        private List<Person> GetPersonList()
        {
            List<Person> persons = new List<Person>();
            persons.Add(new Person() { FirstName = "John", Lastname ="Doe", Age = 24, Gender = 'M' });
            persons.Add(new Person() { FirstName = "Jane", Lastname = "Doe", Age = 30, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Sally", Lastname = "Murphy", Age = 27, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Brian", Lastname = "Dorsey", Age = 32, Gender = 'M' });
            persons.Add(new Person() { FirstName = "William", Lastname = "Fredericks", Age = 50, Gender = 'M' });
            persons.Add(new Person() { FirstName = "Laura", Lastname = "Appletree", Age = 43, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Bob", Lastname = "Stevens", Age = 40, Gender = 'M' });

            return persons;
        }

        #endregion
    }
}
