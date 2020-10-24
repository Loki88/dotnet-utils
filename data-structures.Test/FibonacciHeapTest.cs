using NUnit.Framework;

using System;
using data_structures.Classes.Queue;
using data_structures.Interfaces.Queue;

namespace Tests
{
    [TestFixture]
    public class FibonacciHeapTest
    {

        private IPriorityQueue<DateTime, string> queue;
        private DateTime date;

        [SetUp]
        public void Setup()
        {
            this.queue = new FibonacciPriorityQueue<DateTime, string>();
            this.date = DateTime.Now;
        }

        [Test]
        public void TestInsert()
        {
            Assert.True(queue.Insert(date, "A"));
            Assert.True(queue.Insert(date.AddMinutes(1), "B"));
            Assert.True(queue.Insert(date.AddMinutes(2), "C"));
            Assert.True(queue.Insert(date.AddMinutes(3), "D"));
            Assert.True(queue.Insert(date.AddMinutes(4), "E"));
            Assert.True(queue.Insert(date.AddMinutes(5), "F"));
            Assert.AreEqual(6, queue.Count());

            Assert.True(queue.Delete("B"));
            Assert.True(queue.Delete("D"));
            Assert.False(queue.Delete("a"));
            Assert.False(queue.Delete("B"));
            
        }

        [Test]
        public void TestDecreaseKey()
        {
            Assert.True(queue.Insert(date, "A"));
            Assert.True(queue.Insert(date.AddMinutes(1), "B"));
            Assert.True(queue.Insert(date.AddMinutes(2), "C"));
            Assert.True(queue.Insert(date.AddMinutes(3), "D"));
            Assert.True(queue.Insert(date.AddMinutes(4), "E"));
            Assert.True(queue.Insert(date.AddMinutes(5), "F"));
            Assert.AreEqual(6, queue.Count());

            string val = string.Empty;
            DateTime key;

            bool boolVal = queue.DecreaseKey("E", date.AddMinutes(-7));
            Assert.True(boolVal);

            boolVal = queue.FindMin(out val);
            Assert.True(boolVal);

            boolVal = queue.DeleteMin(out val, out key);
            Assert.True(boolVal);

            Assert.AreEqual("E", val);
        }

        [Test]
        public void TestIncreaseKey()
        {
            Assert.True(queue.Insert(date, "A"));
            Assert.True(queue.Insert(date.AddMinutes(1), "B"));
            Assert.True(queue.Insert(date.AddMinutes(2), "C"));
            Assert.True(queue.Insert(date.AddMinutes(3), "D"));
            Assert.True(queue.Insert(date.AddMinutes(4), "E"));
            Assert.True(queue.Insert(date.AddMinutes(5), "F"));
            Assert.AreEqual(6, queue.Count());

            string val = string.Empty;
            DateTime key;

            bool boolVal = queue.IncreaseKey("A", date.AddMinutes(7));
            Assert.True(boolVal);

            boolVal = queue.FindMin(out val);
            Assert.True(boolVal);

            boolVal = queue.DeleteMin(out val, out key);
            Assert.True(boolVal);

            Assert.AreEqual("B", val);
        }
    }
}