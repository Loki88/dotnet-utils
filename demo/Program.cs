using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using data_structures.Classes.Graph;
using data_structures.Classes.Queue;
using data_structures.Interfaces.Queue;
using IntervalTree;

namespace demo
{
    class Program
    {
        static void TestPriorityQueue()
        {
            IPriorityQueue<DateTime, string> queue = new ConcurrentFibonacciPriorityQueue<DateTime, string>();
            DateTime date = DateTime.Now;

            queue.Insert(date, "A");
            queue.Insert(date.AddMinutes(1), "B");
            queue.Insert(date.AddMinutes(2), "C");
            queue.Insert(date.AddMinutes(3), "D");
            queue.Insert(date.AddMinutes(4), "E");
            queue.Insert(date.AddMinutes(5), "F");

            Console.WriteLine(String.Format("DecreaseKey {0}", queue.DecreaseKey("E", date.AddMinutes(-7))));
            Console.WriteLine(String.Format("IncreaseKey {0}", queue.IncreaseKey("A", date.AddMinutes(7))));

            queue.Delete("D");

            while (!queue.IsEmpty())
            {
                string val;
                DateTime key;
                queue.DeleteMin(out val, out key);

                Console.WriteLine(String.Format("Min val is {0} with key {1}", val, key.ToString()));
            }
        }

        static void TestGraph()
        {
            RangeElement[] elements = new RangeElement[]{
                new RangeElement(){
                    Name = "A",
                    Start = new DateTimeOffset(2020, 10, 24, 0, 0, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 24, 10, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 0, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 24, 8, 0, 0, 0, TimeSpan.FromHours(2)),
                },
                new RangeElement(){
                    Name = "B",
                    Start = new DateTimeOffset(2020, 10, 24, 10, 0, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 24, 10, 45, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 8, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 24, 11, 0, 0, 0, TimeSpan.FromHours(2)),
                },
                new RangeElement(){
                    Name = "C",
                    Start = new DateTimeOffset(2020, 10, 24, 10, 45, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 24, 11, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 11, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 24, 18, 0, 0, 0, TimeSpan.FromHours(2)),
                },
                new RangeElement(){
                    Name = "D",
                    Start = new DateTimeOffset(2020, 10, 24, 11, 0, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 24, 19, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 18, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 24, 19, 0, 0, 0, TimeSpan.FromHours(2)),
                },
                new RangeElement(){
                    Name = "E",
                    Start = new DateTimeOffset(2020, 10, 24, 19, 0, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 24, 21, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 21, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 25, 0, 0, 0, 0, TimeSpan.FromHours(2)),
                },
                new RangeElement(){
                    Name = "F",
                    Start = new DateTimeOffset(2020, 10, 24, 21, 0, 0, 0, TimeSpan.FromHours(2)),
                    End = new DateTimeOffset(2020, 10, 25, 0, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldStart = new DateTimeOffset(2020, 10, 24, 19, 0, 0, 0, TimeSpan.FromHours(2)),
                    OldEnd = new DateTimeOffset(2020, 10, 24, 21, 0, 0, 0, TimeSpan.FromHours(2)),
                },
            };

            Graph<RangeElement> graph = new Graph<RangeElement>(elements);
            IIntervalTree<DateTimeOffset, RangeElement> tree = new IntervalTree.IntervalTree<DateTimeOffset, RangeElement>();
            foreach (RangeElement e in elements)
            {
                tree.Add(e.OldStart, e.OldEnd, e);
            }

            foreach (RangeElement e in elements)
            {
                IEnumerable<RangeElement> overlappingElements = tree.Query(e.Start, e.End)
                    .Where(x => !x.Name.Equals(e.Name) && x.OldStart.CompareTo(e.End) < 0 && x.OldEnd.CompareTo(e.Start) > 0);
                graph.AddOutgoingRelations(e, overlappingElements);
            }

            HashSet<IEnumerable<RangeElement>> cycles = null;
            int k = 1;
            while (graph.HasCycles(out cycles))
            {
                Console.WriteLine(String.Format("#{0} - Graph is cyclic. The following are cycles: ", k));
                int j = 1;
                foreach (IEnumerable<RangeElement> cycle in cycles)
                {
                    Console.WriteLine(String.Format("#{0} - {1}", j, string.Join(" -> ", cycle.Select(x => x.Name))));
                    j++;
                    RangeElement latest = cycle.OrderByDescending(x => x.Start).First();
                    graph.Remove(latest);
                }
                k++;
            }

            IEnumerable<RangeElement> timeSortedChanges = graph.PriorityVisit<int>(
                (GraphNode<RangeElement> e) =>
                {
                    return e.OutDegree;
                },
                (int key, GraphNode<RangeElement> el) =>
                {
                    return key - 1;
                }
            );

            int i = 1;
            Console.WriteLine(String.Format("Change elements in this way:"));
            foreach (RangeElement el in timeSortedChanges)
            {
                Console.WriteLine(String.Format("#{0} - Element {1} -> From: {2}-{3} To: {4}-{5}", i, el.Name,
                    el.OldStart.ToString("o"), el.OldEnd.ToString("o"), el.Start.ToString("o"), el.End.ToString("o")));
                i++;
            }
        }

        static void Main(string[] args)
        {
            // TestPriorityQueue();
            TestGraph();
        }
    }


    public class RangeElement
    {
        public string Name { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
        public DateTimeOffset OldStart { get; set; }
        public DateTimeOffset OldEnd { get; set; }
    }
}
