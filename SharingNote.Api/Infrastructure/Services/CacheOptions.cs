using Microsoft.Extensions.Caching.Distributed;

namespace SharingNote.Api.Infrastructure.Services
{
    public class CacheOptions
    {
        public static DistributedCacheEntryOptions DefaultExpiration => new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        public static DistributedCacheEntryOptions Create(TimeSpan? expiration) =>
        expiration != null ? new()
        {
            AbsoluteExpirationRelativeToNow = expiration
        } : DefaultExpiration;
    }
}
