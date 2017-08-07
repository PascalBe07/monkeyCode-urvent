using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyCode.Framework.Portable.Urvent.Services.Core
{
    public class MockDataService : IDataService
    {
        private readonly IDictionary<Type, List<object>> _data;

        public MockDataService()
        {
            this._data = new Dictionary<Type, List<object>>();
        }

        public void AddItem<T>(T item)
        {
            var key = typeof (T);

            if (this._data.ContainsKey(key))
            {
                this._data[key].Add(item);
                return;
            }

            this._data.Add(key, new List<object> { item });
        }

        public T[] GetItems<T>() where T : class
        {
            var key = typeof (T);
            if (!this._data.ContainsKey(key))
            {
                return new T[0];
            }

            var result = this._data[key].Cast<T>().ToArray();
            return result;
        }
    }
}