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

        [SetUp]
        public void SetUp()
        {
            _array = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            _persons = GetPersonList();
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
        public void CanThrowExceptionOEmptyCollectionEnumerable()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            Assert.Throws<InvalidOperationException>(
                () => noPersons.Partition<Person>(x => x.Gender == 'M', true));
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
        public void CanThrowExceptionOnInvalidChunkSize()
        {
            IEnumerable<IList<Person>> chunked = _persons.Chunk(0);
            Assert.Throws<InvalidOperationException>(
                () => chunked.Count());
        }

        [Test]
        public void CanThrowExceptionIfCollectionIsEmpty()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<IList<Person>> chunked = emptyList.Chunk(1, true);
            Assert.Throws<InvalidOperationException>(
                () => chunked.Count());
        }

        [Test]
        public void CanReturnEmptyCollectionIfCalledOnEmptyCollection()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<IList<Person>> chunked = emptyList.Chunk(1, false);
            Assert.IsEmpty(chunked);
        }

        #endregion

        #region Tests for Init

        [Test]
        public void CanInit()
        {
            IEnumerable<int> nums = _array.Init<int>();
            Assert.IsTrue(nums.Count() == _array.Length - 1);
            // is last element in new collection the second from last in the original?
            Assert.IsFalse(nums.ElementAt(nums.Count() - 1) == _array[_array.Length - 1]);
        }

        [Test]
        public void CanReturnEmptyCollectionIfInitCalledOnEmptyCollection()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<Person> init = emptyList.Init<Person>();
            Assert.IsEmpty(init);
        }

        #endregion

        #region Tests for Tail

        [Test]
        public void CanTail()
        {
            IEnumerable<int> nums = _array.Tail<int>();
            Assert.IsTrue(nums.Count() == _array.Length - 1);
            Assert.IsTrue(nums.ElementAt(0) == _array[1]);
        }

        [Test]
        public void CanReturnEmptyCollectionIfTailCalledOnEmptyCollection()
        {
            IList<Person> emptyList = new List<Person>();
            IEnumerable<Person> tail = emptyList.Tail<Person>();
            Assert.IsEmpty(tail);
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

    [TestFixture]
    public class TestJaccardIndexMethods
    {
        IList<int> _jaccard1;
        IList<int> _jaccard2;
        IList<int> _jaccard3;

        [SetUp]
        public void SetUp()
        {
            _jaccard1 = new List<int> { 4, 5, 6, 7, 8 };
            _jaccard2 = new List<int> { 8, 9, 10, 11, 12 };
            _jaccard3 = new List<int> { 1, 2, 3, 4, 5 };
        }

        #region tests for JaccardIndexSort

        [Test]
        public void CanGetJaccardIndex()
        {
            //non-binary variant implementation
            double jaccardIndex1 = _jaccard1.GetJaccardIndex<int>(_jaccard3);
            Assert.AreEqual((double)2 / 8, jaccardIndex1);

            double jaccardIndex2 = _jaccard1.GetJaccardIndex<int>(_jaccard2);
            Assert.AreEqual((double)1 / 9, jaccardIndex2);

            Person person1 = new Person() { FirstName = "John", Lastname = "Doe", Age = 24, Gender = 'M' };
            Person person2 = new Person() { FirstName = "Val", Lastname = "Murphy", Age = 30, Gender = 'F' };

            //binary variant implementation
            double jaccardIndex3 = person1.GetJaccardIndex<Person>(new List<Func<Person, bool>>() {
                                                            (x) => x.FirstName == "John", 
                                                            (x) => x.Lastname == "Murphy",
                                                            (x) => x.Age < 30,
                                                            (x) => x.Gender == 'M' });

            Assert.AreEqual((double)3 / 4, jaccardIndex3);

            double jaccardIndex4 = person2.GetJaccardIndex<Person>(new List<Func<Person, bool>>() {
                                                            (x) => x.FirstName == "John", 
                                                            (x) => x.Lastname == "Murphy",
                                                            (x) => x.Age < 30,
                                                            (x) => x.Gender == 'M' });
            Assert.AreEqual((double)1 / 4, jaccardIndex4);
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfParamsNullBinaryDataVersion()
        {
            Person nullPerson = null;
            Person person1 = new Person() { FirstName = "John", Lastname = "Doe", Age = 24, Gender = 'M' };

            Assert.Throws<ArgumentNullException>(
                    () => nullPerson.GetJaccardIndex<Person>(new List<Func<Person, bool>>() {
                                                            (x) => x.FirstName == "John", 
                                                            (x) => x.Lastname == "Murphy",
                                                            (x) => x.Age < 30,
                                                            (x) => x.Gender == 'M' }));

            Assert.Throws<ArgumentNullException>(
                    () => nullPerson.GetJaccardIndex<Person>(null));
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfComparatorsEmpty()
        {
            Person person1 = new Person() { FirstName = "John", Lastname = "Doe", Age = 24, Gender = 'M' };

            Assert.Throws<InvalidOperationException>(
                    () => person1.GetJaccardIndex(new List<Func<Person, bool>>()));
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfParamsNullEnumerableDataVersion()
        {
            IList<int> listIsNull = null;

            Assert.Throws<ArgumentNullException>(
                    () => listIsNull.GetJaccardIndex<int>(_jaccard1).ToString());

            Assert.Throws<ArgumentNullException>(
                    () => _jaccard1.GetJaccardIndex<int>(listIsNull).ToString());
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfParamsEmptyOnGetIndex()
        {
            IList<int> emptyList = new List<int>();

            Assert.Throws<InvalidOperationException>(
                    () => emptyList.GetJaccardIndex<int>(_jaccard1).ToString());

            Assert.Throws<InvalidOperationException>(
                    () => _jaccard1.GetJaccardIndex<int>(emptyList).ToString());
        }

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

            //binary variant implementation
            IList<Person> persons = GetPersonList();

            var sortedList = persons.JaccardIndexSort<Person>(
                                                        new List<Func<Person, bool>>() {
                                                            (x) => x.FirstName == "John", 
                                                            (x) => x.Lastname.StartsWith("D"),
                                                            (x) => x.Age < 30,
                                                            (x) => x.Gender == 'M' });

            Assert.AreEqual(sortedList.ElementAt(0).FirstName, "John");
            Assert.AreEqual(sortedList.ElementAt(1).FirstName, "Brian");
            Assert.AreEqual(sortedList.ElementAt(2).FirstName, "Bob");
            Assert.AreEqual(sortedList.Last().FirstName, "Laura");
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfParamsNullOnSort()
        {
            IList<int> nullList = null;

            IList<IList<int>> lists = new List<IList<int>>();
            lists.Add(_jaccard1);
            lists.Add(_jaccard2);
            lists.Add(_jaccard3);

            Assert.Throws<ArgumentNullException>(
                    () => nullList.JaccardIndexSort<int>(lists).Count());

            IList<IList<int>> nullListOfLists = null;

            Assert.Throws<ArgumentNullException>(
                    () => _jaccard1.JaccardIndexSort<int>(nullListOfLists).Count());
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfParamsEmptyOnSort()
        {
            IList<int> emptyList = new List<int>();

            IList<IList<int>> lists = new List<IList<int>>();
            lists.Add(_jaccard1);
            lists.Add(_jaccard2);
            lists.Add(_jaccard3);

            Assert.Throws<InvalidOperationException>(
                    () => emptyList.JaccardIndexSort<int>(lists).Count());

            IList<IList<int>> emptyListOfLists = new List<IList<int>>();

            Assert.Throws<InvalidOperationException>(
                    () => _jaccard1.JaccardIndexSort<int>(emptyListOfLists).Count());
        }

        [Test]
        public void WillThrowArgumentNullExceptionIfParamsNullOnSortBinary()
        {
            IList<Person> persons = GetPersonList();
            IList<Func<Person, bool>> nullList = null;

            Assert.Throws<ArgumentNullException>(
                () => persons.JaccardIndexSort<Person>(nullList).Count());

            persons = null;
            Assert.Throws<ArgumentNullException>(
                () => persons.JaccardIndexSort<Person>(new List<Func<Person, bool>>() {
                                                            (x) => x.FirstName == "John", 
                                                            (x) => x.Lastname == "Murphy",
                                                            (x) => x.Age < 30,
                                                            (x) => x.Gender == 'M' }).Count());
        }

        [Test]
        public void WillThrowInvalidOperationExceptionIfComparatorsEmptyOnSortBinary()
        {
            IList<Person> persons = GetPersonList();

            Assert.Throws<InvalidOperationException>(
                () => persons.JaccardIndexSort<Person>(new List<Func<Person, bool>>() { }).Count());
        }

        #endregion

        private List<Person> GetPersonList()
        {
            List<Person> persons = new List<Person>();

            persons.Add(new Person() { FirstName = "Laura", Lastname = "Appletree", Age = 43, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Bob", Lastname = "Stevens", Age = 40, Gender = 'M' });
            persons.Add(new Person() { FirstName = "Jane", Lastname = "Doe", Age = 30, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Sally", Lastname = "Murphy", Age = 27, Gender = 'F' });
            persons.Add(new Person() { FirstName = "Brian", Lastname = "Dorsey", Age = 32, Gender = 'M' });
            persons.Add(new Person() { FirstName = "William", Lastname = "Fredericks", Age = 50, Gender = 'M' });
            persons.Add(new Person() { FirstName = "John", Lastname = "Doe", Age = 24, Gender = 'M' });

            return persons;
        }
    }

    #region nested class

    class Person
    {
        internal string FirstName { get; set; }
        internal string Lastname { get; set; }
        internal char Gender { get; set; }
        internal int Age { get; set; }
    }

    #endregion
}
