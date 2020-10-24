using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using data_structures.Interfaces.Queue;

namespace data_structures.Classes.Queue
{
    public class ConcurrentFibonacciPriorityQueue<J, T> : IPriorityQueue<J, T>
        where J : IComparable<J>
    {
        private FibonacciPriorityQueue<J, T> PriorityQueue;
        private Mutex Mut;


        public ConcurrentFibonacciPriorityQueue()
        {
            this.PriorityQueue = new FibonacciPriorityQueue<J, T>();
            this.Mut = new Mutex();
        }

        public int Count()
        {
            int count = 0;
            try
            {
                this.Mut.WaitOne();
                count = this.PriorityQueue.Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - Count: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return count;
        }

        public bool DecreaseKey(T value, J decr)
        {
            bool executed = false;
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.DecreaseKey(value, decr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - DecreaseKey: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public bool Delete(T value)
        {
           bool executed = false;
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.Delete(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - Delete: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public bool DeleteMin(out T value, out J key)
        {
            bool executed = false;
            value = default(T);
            key = default(J);
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.DeleteMin(out value, out key);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - DeleteMin: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public bool FindMin(out T value)
        {
            bool executed = false;
            value = default(T);
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.FindMin(out value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - FindMin: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> enumerator = null;
            try
            {
                this.Mut.WaitOne();
                enumerator = this.PriorityQueue.GetEnumerator();
                List<T> copy = new List<T>();
                while(enumerator.MoveNext())
                {
                    copy.Add(enumerator.Current);
                }
                enumerator.Dispose();
                enumerator = copy.GetEnumerator();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - IncreaseKey: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return enumerator;
        }

        public bool IncreaseKey(T value, J incr)
        {
            bool executed = false;
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.IncreaseKey(value, incr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - IncreaseKey: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public bool Insert(J key, T value)
        {
            bool executed = false;
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.Insert(key, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - Insert: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        public bool IsEmpty()
        {
            bool empty = true;
            try
            {
                this.Mut.WaitOne();
                empty = this.PriorityQueue.IsEmpty();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - IsEmpty: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return empty;
        }

        public bool Merge(IPriorityQueue<J, T> q1, IPriorityQueue<J, T> q2)
        {
            bool executed = false;
            try
            {
                this.Mut.WaitOne();
                executed = this.PriorityQueue.Merge(q1, q2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in ConcurrentFibonacciPriorityQueue - Merge: ", ex);
            }
            finally
            {
                this.Mut.ReleaseMutex();
            }
            return executed;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}