using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using EnumerationExtensions;

namespace ExtensionMethodTests
{
    [TestFixture]
    public class TestEnumerableMethods
    {
        int[] _array;
        List<Person> _persons;
        IList<int> _jaccard1;
        IList<int> _jaccard2;
        IList<int> _jaccard3;

        [SetUp]
        public void SetUp()
        {
            _array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            _persons = GetPersonList();
            _jaccard3 = new List<int> { 1,2,3,4,5 };
            _jaccard1 = new List<int> { 4,5,6,7,8 };
            _jaccard2 = new List<int> { 8,9,10,11,12 };
        }

        #region Tests for Partition

        [Test]
        public void CanPartitionEnumerableOfPrimitiveType()
        {
            IEnumerable<int>[] partitionedResults1 = _array.Partition<int>(x => x > 5);
            Assert.AreEqual(partitionedResults1.Length, 2);
            Assert.AreEqual(partitionedResults1[0].Count(), 4);
            Assert.AreEqual(partitionedResults1[1].Count(), 6);
        }

        [Test]
        public void CanPartitionEnumerableOfComplexType()
        {
            IEnumerable<Person>[] partitionedResults = _persons.Partition<Person>(x => x.Gender == 'M');
            Assert.AreEqual(partitionedResults.Length, 2);
            Assert.AreEqual(partitionedResults[0].Count(), 4);
            Assert.AreEqual(partitionedResults[1].Count(), 3);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanThrowExceptionOEmptyCollectionEnumerable()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            IEnumerable<Person>[] partitionedResults =
                        noPersons.Partition<Person>(x => x.Gender == 'M', true);
        }

        [Test]
        public void CanReturnEmptyCollectionIfEmptyCollectionEnumerable()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            IEnumerable<Person>[] partitionedResults =
                        noPersons.Partition<Person>(x => x.Gender == 'M', false);

            Assert.IsTrue(partitionedResults[0].Count() == 0);
            Assert.IsTrue(partitionedResults[1].Count() == 0);
        }

        #endregion

        #region Tests for Chunk

        [Test]
        public void CanChunk()
        {
            int chunkSize = 2;
            IList<Person> persons = GetPersonList();
            IEnumerable<IList<Person>> chunkedData = persons.Chunk<Person>(chunkSize);
            Assert.AreEqual(4, chunkedData.Count());
            Assert.AreEqual(chunkSize, chunkedData.ElementAt(0).Count());
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanThrowExceptionOnInvalidChunkSize()
        {
            IEnumerable<IList<Person>> chunked = _persons.Chunk(0);
            var test = chunked.Count(); //deferred execution means requires action to raise exception 
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CanThrowExceptionIfCollectionIsEmpty()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<IList<Person>> chunked = emptyList.Chunk(1, true);
            var test = chunked.Count(); //deferred execution means requires action to raise exception 
        }

        [Test]
        public void CanReturnEmptyCollectionIfCalledOnEmptyCollection()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<IList<Person>> chunked = emptyList.Chunk(1, false);
            Assert.IsEmpty(chunked);
        }

        #endregion

        #region tests for JaccardIndexSort

        [Test]
        public void CanJaccardIndexSort()
        {
            IList<int> list = new List<int>() { 1, 2, 3, 4, 20 };

            IList<IList<int>> lists = new List<IList<int>>();
            lists.Add(_jaccard1);
            lists.Add(_jaccard2);
            lists.Add(_jaccard3);

            var sortedLists = list.JaccardIndexSort<int>(lists);
            Assert.AreEqual(_jaccard3, sortedLists.ElementAt(0));
            Assert.AreEqual(_jaccard1, sortedLists.ElementAt(1));
            Assert.AreEqual(_jaccard2, sortedLists.ElementAt(2));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WillThrowArgumentNullExceptionIfSourceIsNull()
        {
            IList<int> nullList = null;

            IList<IList<int>> lists = new List<IList<int>>();
            lists.Add(_jaccard1);
            lists.Add(_jaccard2);
            lists.Add(_jaccard3);

            var sortedLists = nullList.JaccardIndexSort<int>(lists);
            var forceError = sortedLists.Count();
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WillThrowArgumentNullExceptionIfCompareToIsNull()
        {
            IList<IList<int>> nullLists = null;

            var sortedLists = _jaccard1.JaccardIndexSort<int>(nullLists);
            var forceError = sortedLists.Count();
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillThrowInvalidOperationExceptionIfListIsEmpty()
        {
            IList<int> emptyList = new List<int>();

            IList<IList<int>> lists = new List<IList<int>>();
            lists.Add(_jaccard1);
            lists.Add(_jaccard2);
            lists.Add(_jaccard3);

            var sortedLists = emptyList.JaccardIndexSort<int>(lists);
            var forceError = sortedLists.Count();

        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WillThrowInvalidOperationExceptionIfCompareToIsEmpty()
        {
            IList<IList<int>> emptyCompareTo = new List<IList<int>>();

            var sortedLists = _jaccard1.JaccardIndexSort<int>(emptyCompareTo);
            var forceError = sortedLists.Count();
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
            persons.Add(new Person() { FirstName = "John", Lastname = "Doe", Age = 24, Gender = 'M' });
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
