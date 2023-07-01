using GoldinAccountManager.RedisCache.Interface;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace GoldinAccountManager.RedisCache.Abstract
{
    public class CacheHelperRespository : ICacheHelperRespository
    {

        public async Task SetRecordAsync<T>(IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
                options.SlidingExpiration = slidingExpireTime;

                var jsonData = JsonSerializer.Serialize(data);
                await cache.SetStringAsync(recordId, jsonData, options);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<T?> GetRecordAsync<T>(IDistributedCache cache, string recordId)
        {
            try
            {
                var jsonData = await cache.GetStringAsync(recordId);

                if (jsonData is null)
                    return default(T);

                return JsonSerializer.Deserialize<T>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

    }
}
