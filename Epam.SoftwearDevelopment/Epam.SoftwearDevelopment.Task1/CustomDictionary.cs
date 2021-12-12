using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Epam.SoftwearDevelopment.Task1
{
    public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        private LinkedList<KeyValuePair<TKey, TValue>>[] _listOfelem;

        public CustomDictionary()
        {
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[4];
        }

        public CustomDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        public CustomDictionary(CustomDictionary<TKey, TValue> customDictionary)
        {
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[4];
            foreach (var item in customDictionary)
            {
                Add(item);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key.Equals(null))
                {
                    throw new ArgumentNullException("Incorrect key");
                }

                var hash = key.GetHashCode();
                var index = Math.Abs(hash % _listOfelem.Length);
                var elem = _listOfelem[index]?.FirstOrDefault(x => x.Key.Equals(key));
                if (!elem.HasValue)
                {
                    throw new KeyNotFoundException($"Such key {key} not found");
                }

                return elem.Value.Value;
            }
            set
            {
                if (key.Equals(null))
                {
                    throw new ArgumentNullException("Incorrect key");
                }

                if (value.Equals(null))
                {
                    throw new ArgumentNullException("Incorrect value");
                }

                Remove(key);
                Add(key, value);
            }
        }

        public List<TKey> Keys
        {
            get
            {
                var listOfKeys = new List<TKey>();
                for (int i = 0; i < _listOfelem.Length; i++)
                {
                    if (_listOfelem[i] != null)
                    {
                        foreach (var elem in _listOfelem[i])
                        {
                            listOfKeys.Add(elem.Key);
                        }
                    }
                }

                return listOfKeys;
            }
        }

        public List<TValue> Values
        {
            get
            {
                var listOfValues = new List<TValue>();
                for (int i = 0; i < _listOfelem.Length; i++)
                {
                    if (_listOfelem[i] != null)
                    {
                        foreach (var elem in _listOfelem[i])
                        {
                            listOfValues.Add(elem.Value);
                        }
                    }
                }

                return listOfValues;
            }
        }

        public IEqualityComparer<TKey> Comparer { get; }

        public int Count => _listOfelem.Where(x => x != null).Select(x => x.Count).Sum();

        public bool IsReadOnly => false;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (item.Equals(null))
            {
                throw new ArgumentNullException("Incorrect pair");
            }

            var hash = item.Key.GetHashCode();
            var index = Math.Abs(hash % _listOfelem.Length);
            if (_listOfelem[index] is null)
            {
                _listOfelem[index] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            if (_listOfelem[index].Any(x => x.Key.Equals(item.Key)))
            {
                throw new ArgumentException("Argument with such key already exists");
            }

            _listOfelem[index].AddLast(item);

            if (_listOfelem.Any(x => x?.Count > _listOfelem.Length / 3))
            {
                var lenght = _listOfelem.Length * 2;
                var newList = new LinkedList<KeyValuePair<TKey, TValue>>[lenght];
                for (int i = 0; i < _listOfelem.Length; i++)
                {
                    if (_listOfelem[i] != null)
                    {
                        foreach (var elem in _listOfelem[i])
                        {
                            var hashOfElem = elem.Key.GetHashCode();
                            var ind = hashOfElem % lenght;
                            if (newList[ind] is null)
                            {
                                newList[ind] = new LinkedList<KeyValuePair<TKey, TValue>>();
                            }

                            newList[ind].AddLast(elem);
                        }
                    }
                }

                _listOfelem = newList;
            }
        }

        public void Add(TKey key, TValue value)
        {
            var pair = new KeyValuePair<TKey, TValue>(key, value);
            Add(pair);
        }

        public void Clear()
        {
            _listOfelem = new LinkedList<KeyValuePair<TKey, TValue>>[4];
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (item.Key.Equals(null))
            {
                throw new ArgumentNullException($"Incorrect key {item.Key}");
            }

            var hash = item.Key.GetHashCode();
            var ind = hash % _listOfelem.Length;
            if (!_listOfelem[ind].Contains(item))
            {
                return false;
            }

            return true;
        }

        public bool ContainsKey(TKey key)
        {
            if (key.Equals(null))
            {
                throw new ArgumentNullException($"Incorrect key {key}");
            }

            var hash = key.GetHashCode();
            var ind = hash % _listOfelem.Length;
            if (!_listOfelem[ind].Any(x => x.Key.Equals(key)))
            {
                return false;
            }

            return true;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Clear();
            for (int i = arrayIndex; i < array.Length; i++)
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
            if (item.Key.Equals(null))
            {
                throw new ArgumentNullException($"Empty key {item.Key}");
            }

            var hash = item.Key.GetHashCode();
            var index = Math.Abs(hash % _listOfelem.Length);
            if (_listOfelem[index] is null || !_listOfelem[index].Any(x => x.Equals(item)))
            {
                return false;
            }

            _listOfelem[index].Remove(item);

            return true;
        }

        public bool Remove(TKey key)
        {
            if (key.Equals(null))
            {
                throw new ArgumentNullException($"Empty key {key}");
            }

            var hash = key.GetHashCode();
            var index = Math.Abs(hash % _listOfelem.Length);
            var elem = _listOfelem[index]?.FirstOrDefault(x => x.Key.Equals(key));
            if (!elem.HasValue)
            {
                return false;
            }

            _listOfelem[index].Remove(elem.Value);

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            if (key.Equals(null))
            {
                return false;
            }

            var hash = key.GetHashCode();
            var index = hash % _listOfelem.Length;
            var elem = _listOfelem[index]?.FirstOrDefault(x => x.Key.Equals(key));
            if (!elem.HasValue)
            {
                return false;
            }

            value = elem.Value.Value;
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}