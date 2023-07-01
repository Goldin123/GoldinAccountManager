using Microsoft.Extensions.Caching.Distributed;

namespace GoldinAccountManager.RedisCache.Interface
{
    public interface ICacheHelperRespository
    {
        Task SetRecordAsync<T>(IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null);
        Task<T?> GetRecordAsync<T>(IDistributedCache cache, string recordId);
    }
}
