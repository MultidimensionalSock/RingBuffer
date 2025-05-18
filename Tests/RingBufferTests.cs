namespace RingBufferTest
{
    [TestClass]
    public sealed class RingBufferTests
    {
        public TestElement[] testData =
        {
            new TestElement(1),
            new TestElement(2),
            new TestElement(3),
            new TestElement(4),
            new TestElement(5),
            new TestElement(6),
            new TestElement(7),
            new TestElement(8),
            new TestElement(9),
            new TestElement(10),
        };

        [TestMethod]
        public void CreateRingBuffer()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            Assert.IsNotNull(ringBuffer);
        }

        [TestMethod]
        public void AddElements()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            Assert.AreEqual(testData[0], ringBuffer.GetAtIndex(0));
            Assert.AreEqual(testData[1], ringBuffer.GetAtIndex(1));
            Assert.AreEqual(testData[2], ringBuffer.GetAtIndex(2));
            Assert.AreEqual(testData[3], ringBuffer.GetAtIndex(3));
            Assert.AreEqual(true, ringBuffer.NextIndexEmpty);
        }

        [TestMethod]
        public void AddElementsAfter()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            ringBuffer.Add(false, testData[4]);
            Assert.AreEqual(testData[3], ringBuffer.GetAtIndex(3));
            Assert.AreEqual(testData[4], ringBuffer.GetAtIndex(4));
        }

        public void Clear()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            ringBuffer.Clear();
            Assert.IsNull(ringBuffer.GetAtIndex(2));
        }

        [TestMethod]
        public void AddTooManyElements()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(3);
            try
            {
                ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
                Assert.Fail(); // raises AssertionException
            }
            catch (Exception ex)
            {
                Assert.AreEqual("too many elements for ring length", ex.Message);
            }
            Assert.AreEqual(null, ringBuffer.GetAtIndex(0));
            Assert.AreEqual(true, ringBuffer.NextIndexEmpty);
        }

        [TestMethod]
        public void OverwriteExistingdata()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(5);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            try
            {
                ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
                Assert.Fail(); // raises AssertionException
            }
            catch (Exception ex)
            {
                Assert.AreEqual("This would overwrite Existing data", ex.Message);
            }
            Assert.IsNull(ringBuffer.GetAtIndex(4));
        }

        [TestMethod]
        public void AddAtIndex()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            Assert.AreEqual(testData[0], ringBuffer.GetAtIndex(2));
            Assert.AreEqual(testData[1], ringBuffer.GetAtIndex(3));
            Assert.AreEqual(testData[2], ringBuffer.GetAtIndex(4));
            Assert.AreEqual(testData[3], ringBuffer.GetAtIndex(5));
            Assert.AreEqual(true, ringBuffer.NextIndexEmpty);
        }

        [TestMethod]
        public void NextWriteIndexEmpty()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            Assert.AreEqual(true, ringBuffer.NextIndexEmpty);
        }

        [TestMethod]
        public void MoveNext()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            Assert.AreEqual(true, ringBuffer.NextIndexEmpty);
        }

        [TestMethod]
        public void RemoveAtIndex()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            ringBuffer.Remove(0);
            ringBuffer.Remove(2);
            Assert.AreEqual(null, ringBuffer.GetAtIndex(0));
            Assert.AreEqual(testData[1], ringBuffer.GetAtIndex(1));
            Assert.AreEqual(null, ringBuffer.GetAtIndex(2));
            Assert.AreEqual(testData[3], ringBuffer.GetAtIndex(3));
        }

        [TestMethod]
        public void RemoveByElement()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(9);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            ringBuffer.Remove(testData[1]);
            Assert.AreEqual(testData[0], ringBuffer.GetAtIndex(0));
            Assert.AreEqual(null, ringBuffer.GetAtIndex(1));
            Assert.AreEqual(testData[2], ringBuffer.GetAtIndex(2));
            Assert.AreEqual(testData[3], ringBuffer.GetAtIndex(3));
        }

        [TestMethod]
        public void RemoveElementNotFound()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(4);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            try
            {
                ringBuffer.Remove(testData[5]);
                Assert.Fail(); // raises AssertionException
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Element Not Found in buffer", ex.Message);
            }
        }

        [TestMethod]
        public void MoveBack()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(4);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            ringBuffer.MoveBack();
            Assert.AreEqual(ringBuffer.Current, testData[3]);
        }

        [TestMethod]
        public void Contains()
        {
            RingBuffer.RingBuffer<TestElement> ringBuffer = new RingBuffer.RingBuffer<TestElement>(4);
            ringBuffer.Add(false, testData[0], testData[1], testData[2], testData[3]);
            Assert.IsTrue(ringBuffer.Contains(testData[3]));
            Assert.IsFalse(ringBuffer.Contains(testData[5]));
        }
    }

    public class TestElement
    {
        public int value;

        public TestElement(int val)
        { value = val; }
    }
}
