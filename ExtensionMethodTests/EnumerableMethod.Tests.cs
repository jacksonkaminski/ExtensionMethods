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
                        noPersons.Partition<Person>(x => x.Gender == 'M', OnEmptyCollection.ThrowException);
        }

        [Test]
        public void CanReturnEmptyCollectionIfEmptyCollectionEnumerable()
        {
            IEnumerable<Person> noPersons = new List<Person>();
            IEnumerable<Person>[] partitionedResults =
                        noPersons.Partition<Person>(x => x.Gender == 'M', OnEmptyCollection.ReturnEmptyCollections);

            Assert.IsTrue(partitionedResults[0].Count() == 0);
            Assert.IsTrue(partitionedResults[1].Count() == 0);
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
