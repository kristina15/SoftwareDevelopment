using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epam.SoftwearDevelopment.Task1
{
    public class CustomDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private List<TKey> _keysList = null;
        private List<LinkedList<TValue>> _valuesList = null;

        public CustomDictionary()
        {
            _keysList = new List<TKey>();
            _valuesList = new List<LinkedList<TValue>>();
        }

        public CustomDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public CustomDictionary(CustomDictionary<TKey, TValue> customDictionary)
        {
            _keysList.AddRange(customDictionary.Keys);
            _valuesList.AddRange(customDictionary.Values);
        }

        public List<TKey> Keys { get; }

        public List<LinkedList<TValue>> Values { get; }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException($"Empty key {key}");
                }

                int ind = _keysList.IndexOf(key);
                if (ind == -1)
                {
                    throw new KeyNotFoundException($"Key {key} not found");
                }

                return _valuesList[ind].LastOrDefault();
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException($"Empty key {key}");
                }

                int ind = _keysList.IndexOf(key);
                if (ind == -1)
                {
                    throw new KeyNotFoundException($"Key {key} not found");
                }

                _valuesList[ind].AddLast(value);
            }
        }

        public IEqualityComparer<TKey> Comparer { get; }

        public int Count => _keysList.Count;

        public bool IsReadOnly => false;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            int ind = _keysList.IndexOf(item.Key);
            if (ind != -1)
            {
                _valuesList[ind].AddLast(item.Value);
            }
            else
            {
                _keysList.Add(item.Key);
                _valuesList.Add(new LinkedList<TValue>());
                _valuesList[Count - 1].AddLast(item.Value);
            }
        }

        public void Clear()
        {
            _keysList.Clear();
            _valuesList.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException($"Empty key {item.Key}");
            }

            int ind = _keysList.IndexOf(item.Key);
            if (ind == -1)
            {
                return false;
            }

            if (_valuesList[ind].Contains(item.Value))
            {
                return true;
            }

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < _keysList.Count; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(_keysList[i], _valuesList[i].Last());
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentNullException($"Empty key {item.Key}");
            }

            int ind = _keysList.IndexOf(item.Key);
            if (ind == -1)
            {
                throw new KeyNotFoundException($"Key {item.Key} not found");
            }

            if (!_valuesList[ind].Contains(item.Value))
            {
                throw new ArgumentException($"Value {item.Value} not found");
            }

            _valuesList[ind].Remove(item.Value);
            if (_valuesList[ind].Count == 0)
            {
                _keysList.Remove(item.Key);
            }

            return true;

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
