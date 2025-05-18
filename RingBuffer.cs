// Created by Robyn Botham, 2025

namespace RingBuffer
{
    public class RingBuffer<T> where T : class
    {

        public uint Length = 0;
        public System.Action RingBufferFull;
        protected T[] _buffer;
        protected uint _index;

        public RingBuffer(uint capacity)
        {
            _buffer = new T[capacity];
            Length = capacity;
        }

        public T Current
        {
            get => _buffer[_index];
            private set => _buffer[_index] = value;
        }

        public bool NextIndexEmpty => IsNextEmpty(_index);

        public bool Add(bool allowOverwrite, params T[] elements)
        {
            if (!allowOverwrite)
            {
                uint newindex = _index + (uint)elements.Length;
                if (newindex >= Length)
                    newindex -= Length;
                if (_buffer[newindex] != null)
                    throw new Exception("This would overwrite Existing data");
            }
            if (elements.Length > Length)
            {
                throw new ArgumentException("too many elements for ring length");
            }
            foreach (T element in elements)
            {
                Current = element;
                _index++;
            }
            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < Length; i++)
            {
                _buffer[i] = default;
            }
            _index = 0;
        }

        public bool Contains(T element)
        {
            return Array.IndexOf(_buffer, element) != -1;
        }

        public bool CopyTo(int index, params T[] elements)
        {
            if (elements.Length > Length)
            {
                throw new ArgumentException("too many elements for ring length");
            }
            foreach (T element in elements)
            {
                _buffer[index++] = element;
            }
            return true;
        }
        public T GetAtIndex(int index)
        {
            return _buffer[index];
        }

        public void MoveBack()
        {
            if (_index == 0)
            {
                _index = Length;
                return;
            }
            _index--;
        }

        public void MoveNext()
        {
            _index = GetNextIndex(_index);
        }
        public void Remove(int index)
        {
            _buffer[index] = default;
        }

        public void Remove(T element)
        {
            int index = Array.IndexOf(_buffer, element);
            if (index < 0)
            {
                throw new Exception("Element Not Found in buffer");
            }
            _buffer[index] = default;
        }

        protected uint GetNextIndex(uint currentIndex)
        {
            if (currentIndex >= Length)
            {
                return 0;
            }
            return currentIndex++;
        }

        protected bool IsNextEmpty(uint index)
        {
            return _buffer[GetNextIndex(index)] == default;
        }

    }
}
