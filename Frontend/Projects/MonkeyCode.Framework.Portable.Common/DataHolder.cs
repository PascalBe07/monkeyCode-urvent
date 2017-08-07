using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MonkeyCode.Framework.Portable.Common
{
    public class DataHolder
    {
        private readonly IDictionary<string, object> _cache;

        public DataHolder()
        {
            this._cache = new Dictionary<string, object>();
        }

        protected TResult GetLazy<TResult>(Func<TResult> getDefault = null,
            [CallerMemberName] string propertyName = null)
        {
            getDefault = getDefault ?? (() => default(TResult));

            if (!this._cache.ContainsKey(propertyName))
            {
                var value = getDefault();
                this._cache.Add(propertyName, value);

                return value;
            }

            return (TResult)this._cache[propertyName];
        }
    }
}
