using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using data_structures.Classes.Queue;
using data_structures.Interfaces.Queue;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    public class ConcurrentFibonacciHeapTest {
        private IPriorityQueue<DateTime, string> queue;
        private DateTime date;

        private List<Element> insert;

        private string[] delete;

        [SetUp]
        public void Setup () {
            this.queue = new ConcurrentFibonacciPriorityQueue<DateTime, string> ();
            this.date = DateTime.Now;
            this.insert = new List<Element>();
            this.insert.Add(new Element()
            {
                date = date,
                value = "A"
            });
            this.insert.Add(new Element()
            {
                date = date.AddMinutes(1),
                value = "B"
            });
            this.insert.Add(new Element()
            {
                date = date.AddMinutes(2),
                value = "C"
            });
            this.insert.Add(new Element()
            {
                date = date.AddMinutes(3),
                value = "D"
            });
            this.insert.Add(new Element()
            {
                date = date.AddMinutes(4),
                value = "E"
            });
            this.insert.Add(new Element()
            {
                date = date.AddMinutes(5),
                value = "F"
            });

            this.delete = new string[4]{"B", "D", "a", "B"};

        }

        
        [Test]
        public void TestInsert () {
            
            ParallelLoopResult insert = Parallel.ForEach(this.insert, (Element e) => {
                this.queue.Insert(e.date, e.value);
            });

            Assert.True(insert.IsCompleted);
            Assert.AreEqual (6, queue.Count ());

            ParallelLoopResult delete = Parallel.ForEach(this.delete, (string e) => {
                this.queue.Delete(e);
            });

            Assert.True(delete.IsCompleted);
            Assert.AreEqual (queue.Count (), 4);
        }

        [Test]
        public void TestDecreaseKey()
        {
            ParallelLoopResult insert = Parallel.ForEach(this.insert, (Element e) => {
                this.queue.Insert(e.date, e.value);
            });
            Assert.True(insert.IsCompleted);
            Assert.AreEqual (6, queue.Count ());

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
            ParallelLoopResult insert = Parallel.ForEach(this.insert, (Element e) => {
                this.queue.Insert(e.date, e.value);
            });
            Assert.True(insert.IsCompleted);
            Assert.AreEqual (6, queue.Count ());

            
            string val = string.Empty;
            DateTime key;

            bool boolVal = queue.IncreaseKey("A", date.AddMinutes(7));
            Assert.True(boolVal);

            boolVal = queue.FindMin(out val);
            Assert.True(boolVal);

            boolVal = queue.DeleteMin(out val, out key);
            Assert.True(boolVal);

            Assert.AreEqual( "B", val);
        }
    }

    class Element {
        public DateTime date;
        public string value;
    }
}