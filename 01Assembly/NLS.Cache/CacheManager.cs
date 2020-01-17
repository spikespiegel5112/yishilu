using Microsoft.Extensions.Caching.Memory;
using System;

namespace NLS.Cache
{
    public sealed class CacheManager
    {
        private static IMemoryCache __Cache;

        public CacheManager()
        {
            if (__Cache == null)
            {
                __Cache = new MemoryCache(new MemoryCacheOptions());
            }
        }

        /// <summary>
        /// 根据Key读取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key</param>
        public static T Get<T>(string key) where T : class
        {
            T _t;
            if (__Cache.TryGetValue(key, out _t))
            {
                return _t;
            }
            return null;
        }

        /// <summary>
        /// 根据Key读取缓存,在没有值时,根据_func设置相关缓存信息
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="_func">Func</param>
        public static T Get<T>(string key, Func<T> _func, bool refuse = false) where T : class
        {
            if (refuse)
            {
                Remove(key);
            }
            T result = Get<T>(key);
            if (result == null)
            {
                result = _func();
                if (result != null)
                {
                    Set<T>(key, result);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据Key读取缓存,在没有值时,根据_func设置相关缓存信息
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="func">Func</param>
        /// <param name="timeSpan">缓存时间</param>
        /// <returns></returns>
        public static T Get<T>(string key, Func<T> func, TimeSpan timeSpan) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return default(T);
            }
            T result = Get<T>(key);
            if (result == null)
            {
                result = func();
                if (result != null)
                {
                    Set<T>(key, result, timeSpan);
                }
            }
            return result;
        }

        /// <summary>
        /// 根据Key设置缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="content">T类型的缓存数据</param>
        public static bool Set<T>(string key, T content) where T : class
        {
            if (string.IsNullOrWhiteSpace(key) && content != null)
            {
                __Cache.Set(key, content, new MemoryCacheEntryOptions().SetPriority(CacheItemPriority.Normal));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="content">值类容</param>
        /// <param name="cacheItemPriority">设置缓存回收级别</param>
        /// <returns>true：操作成功  反之失败</returns>
        public static bool Set<T>(string key, T content, CacheItemPriority cacheItemPriority) where T : class
        {
            if (!string.IsNullOrWhiteSpace(key) && content != null)
            {
                __Cache.Set(key, content, new MemoryCacheEntryOptions().SetPriority(cacheItemPriority));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据Key设置缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">Key</param>
        /// <param name="content">值类容</param>
        /// <param name="time">缓存时间</param>
        /// <returns>true：操作成功  反之失败</returns>
        public static bool Set<T>(string key, T content, TimeSpan time) where T : class
        {
            if (!string.IsNullOrWhiteSpace(key) && content != null)
            {
                __Cache.Set(key, content, time);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据Key移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            __Cache.Remove(key);
        }
    }
}