using System;
using System.Collections.Generic;

namespace Roadplus.Server
{
    public class AutoIncrementingDictionary<T>
    {
        private Dictionary<int, T> internalDictionary;
        private int index;

        public AutoIncrementingDictionary()
        {
            internalDictionary = new Dictionary<int, T>();
            index = 0;
        }

        public void Add(T item)
        {
            if (index++ == Int32.MaxValue)
            {
                throw new IndexOutOfRangeException(
                    "Maximum amount of indices reached");
            }

            internalDictionary.Add(index, item);
            index++;
        }

        public void RemoveAt(int index)
        {
            internalDictionary.Remove(index);
        }

        public bool TryGetValue(int index, out T value)
        {
            return internalDictionary.TryGetValue(index, out value);
        }
    }
}

