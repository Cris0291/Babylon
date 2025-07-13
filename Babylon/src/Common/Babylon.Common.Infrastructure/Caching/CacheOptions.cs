using Microsoft.Extensions.Caching.Distributed;

namespace Babylon.Common.Infrastructure.Caching;
public static class CacheOptions
{
    public static DistributedCacheEntryOptions DefaultExpiration() => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
    };
    public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration != null ? new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = expiration } : DefaultExpiration();
}
