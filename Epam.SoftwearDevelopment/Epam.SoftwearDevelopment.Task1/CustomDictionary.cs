using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epam.SoftwearDevelopment.Task1
{
    public class CustomDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private int _capacity = 4;
        private int _count = 0;
        private LinkedList<KeyValuePair<TKey, TValue>>[] _listOfelem;

        public CustomDictionary()
        {
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[_capacity];
        }

        public CustomDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public CustomDictionary(CustomDictionary<TKey, TValue> customDictionary)
        {
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[_capacity];
            foreach (var item in customDictionary)
            {
                Add(item);
            }
        }

        public CustomDictionary(int capacity)
        {
            _capacity = capacity;
        }

        public List<TKey> Keys => _listOfelem.Where(x => x != null).Select(x => x.First.Value.Key).ToList();

        public List<TValue> Values {
            get 
            {
                var list = new List<TValue>();
                for (int i = 0; i < _count; i++)
                {
                    foreach (var item in _listOfelem[i])
                    {
                        list.Add(item.Value);
                    }
                }

                return list;
            }
        }

        public IEqualityComparer<TKey> Comparer { get; }

        public int Count => _count;

        public int Capacity => _capacity;

        public bool IsReadOnly => false;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_listOfelem[i].Any(x => x.Key.Equals(item.Key)))
                {
                    _listOfelem[i].AddLast(item);
                    return;
                }
            }

            if (_capacity == _count)
            {
                _capacity *= 2;
                Array.Resize(ref _listOfelem, _capacity);
            }

            _count++;
            _listOfelem[_count] = new LinkedList<KeyValuePair<TKey, TValue>>();
            _listOfelem[_count].AddLast(item);
        }

        public void Clear()
        {
            _capacity = 4;
            _count = 0;
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[_capacity];
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException($"Empty key {item.Key}");
            }

            if (!_listOfelem.Any(x => x.Contains(item)))
            {
                return false;
            }

            return true;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Clear();
            for (int i = 0; i < array.Length; i++)
            {
                Add(array[i]);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var item in _listOfelem)
            {
                if (item != null)
                {
                    foreach (var item2 in item)
                    {
                        yield return item2;
                    }
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException($"Empty key {item.Key}");
            }

            int ind = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_listOfelem[i].Contains(item))
                {
                    ind = i;
                    break;
                }
            }

            if (ind == -1)
            {
                throw new ArgumentException($"Value {item.Value} not found");
            }

            _listOfelem[ind].Remove(item);
            if (_listOfelem[ind].Count == 0)
            {
                _count--;
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
