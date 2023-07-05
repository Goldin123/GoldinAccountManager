using Microsoft.Extensions.Caching.Distributed;

namespace GoldinAccountManager.RedisCache.Interface
{
    public interface ICacheHelperRespository
    {
        /// <summary>
        /// Interface that allows you to add records into Redis cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="slidingExpireTime"></param>
        /// <returns></returns>
        Task SetRecordAsync<T>(IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null);
        /// <summary>
        /// Interface that allows you to get cache data from Redis.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        Task<T?> GetRecordAsync<T>(IDistributedCache cache, string recordId);
    }
}
