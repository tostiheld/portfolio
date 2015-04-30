using System;
using System.Linq;
using System.Collections.Generic;

namespace Roadplus.Server.API
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

        public int Add(T item)
        {
            if (index++ == Int32.MaxValue)
            {
                throw new IndexOutOfRangeException(
                    "Maximum amount of indices reached");
            }

            internalDictionary.Add(index, item);
            index++;
            return index--;
        }

        public void RemoveAt(int index)
        {
            internalDictionary.Remove(index);
        }

        public bool TryGetValue(int index, out T value)
        {
            return internalDictionary.TryGetValue(index, out value);
        }

        public List<T> ToList()
        {
            return internalDictionary.Values.ToList();
        }
    }
}

