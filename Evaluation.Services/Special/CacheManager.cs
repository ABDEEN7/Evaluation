using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.Services.Special
{
    public class CacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Remove(string key)
        {
            try
            {
                _memoryCache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ContainsKey(string key)
        {
            try
            {
                return _memoryCache.TryGetValue(key, out _);
            }
            catch
            {
                return false;
            }
        }

        public T? GetValue<T>(string key) where T : class
        {
            try
            {
                return _memoryCache.TryGetValue(key, out T? result) ? result : null;
            }
            catch
            {
                return null;
            }
        }

        public bool SetValue<T>(string key, T value, TimeSpan? duration = null) where T : class
        {
            try
            {
                var expiration = duration ?? TimeSpan.FromDays(1);
                _memoryCache.Set(key, value, expiration);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears multiple specific cache keys.
        /// </summary>
        public bool Clear(params string[] keys)
        {
            try
            {
                foreach (var key in keys)
                    _memoryCache.Remove(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears cache entries defined in a static class (fields of type string).
        /// </summary>
        public bool Clear(Type staticClassType)
        {
            try
            {
                var fieldValues = GetFieldValues(staticClassType);
                foreach (var kvp in fieldValues)
                    _memoryCache.Remove(kvp.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clears all cache by creating a new memory cache instance.
        /// </summary>
        public bool ClearAll()
        {
            try
            {
                if (_memoryCache is MemoryCache memCache)
                {
                    memCache.Compact(1.0); // clears all entries safely
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves all static string fields from a static class.
        /// </summary>
        private static Dictionary<string, string> GetFieldValues(Type staticClassType)
        {
            try
            {
                return staticClassType
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(f => f.FieldType == typeof(string))
                    .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
    }
}