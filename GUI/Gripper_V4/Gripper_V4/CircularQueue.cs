using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gripper_V4
{
    internal class CircularQueue<T>   // circular Queue
    {
        private readonly T[] buffer;
        private int head;
        private int tail;
        private int count;

        public int Capacity => buffer.Length;
        public int Count
        {
            get { lock (_lock) return count; }
        }

        private readonly object _lock = new object();

        public CircularQueue(int capacity)
        {
            buffer = new T[capacity];
            head = 0;
            tail = 0;
            count = 0;
        }


        public void Enqueue(T item)
        {
            lock (_lock)
            {
                if (item == null) return;
                buffer[tail] = item;
                tail = (tail + 1) % Capacity;
                if (count == Capacity) head = (head + 1) % Capacity;
                else count++;
            }
        }

        public T Dequeue()
        {
            if (count == 0) throw new InvalidOperationException("Queue is empty");
            T item = buffer[head];
            head = (head + 1) % Capacity;
            count--;
            return item;
        }

        public T? DequeueSafe()
        {
            lock (_lock)
            {
                if (count == 0) return default;
                T item = buffer[head];
                head = (head + 1) % Capacity;
                count--;
                return item;
            }
        }

        public T Peek()
        {
            if (count == 0) throw new InvalidOperationException("Queue is empty");
            return buffer[head];
        }

        public T[] ToArray()
        {
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = buffer[(head + i) % Capacity];
            return result;
        }

        public IEnumerable<T> GetItems()
        {
            for (int i = 0; i < count; i++)
            {
                int index = (head - count + i + buffer.Length) % buffer.Length;

                yield return buffer[index];
            }
        }

        public T? GetLast()
        {
            if (count == 0) return default;
            int index = (tail - 1 + Capacity) % Capacity;
            return buffer[index];
        }

        public void Clear() 
        {
            head = 0;   
            tail = 0;
            count = 0;
        }

    }

}

