using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GoldinAccountManager.API.Helper
{
    public static class CacheHelper
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            await RedisCache.Helper.CacheHelper.SetRecordAsync<T>(cache, recordId, data, absoluteExpireTime, slidingExpireTime);
        }

        public static async Task<T?> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
           return await RedisCache.Helper.CacheHelper.GetRecordAsync<T>(cache, recordId);
        }
    }
}
