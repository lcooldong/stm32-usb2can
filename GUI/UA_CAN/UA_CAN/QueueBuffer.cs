using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UA_CAN
{
    internal class QueueBuffer<T>   // circular Queue
    {
        private readonly T[] _buffer;
        private int _head;
        private int _count;


        public QueueBuffer(int capacity)   
        {
            _buffer = new T[capacity];
            _head = 0;
            _count = 0;
        }

        public void Add(T item) 
        {
            _buffer[_head] = item;
            _head = (_head + 1) % _buffer.Length;
            if( _count < _buffer.Length) _count++;
        }

        public IEnumerable<T> GetItems() 
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_head - _count + i + _buffer.Length) % _buffer.Length;

                yield return _buffer[index];
            }
        }

    }
}
