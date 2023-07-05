using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoldinAccountManager.API.Helper
{
    public static class CacheHelper
    {
        /// <summary>
        /// Sets data to the Redis cache database with a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            await RedisCache.Helper.CacheHelper.SetRecordAsync<T>(cache, recordId, data, absoluteExpireTime, slidingExpireTime);
        }
        /// <summary>
        /// Returns the cached informations from the Redis cache using a given key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
           return await RedisCache.Helper.CacheHelper.GetRecordAsync<T>(cache, recordId);
        }
    }
}
