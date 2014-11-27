using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CollectionExtensions;

namespace ExtensionMethodTests
{
    [TestFixture]
    public class TestsCollectionPartitionMethods
    {
        int[] _array;
        int _size = 10;

        [SetUp]
        public void SetUp()
        {
            _array = new int[] { 0, 1, 2, 3 ,4 ,5 ,6 ,7 ,8 ,9 };
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
    }
}
