using Microsoft.Extensions.Caching.Distributed;
using SharingNote.Api.Application.Services;
using System.Buffers;
using System.Text.Json;

namespace SharingNote.Api.Infrastructure.Services
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var bytes = await cache.GetAsync(key, cancellationToken);
            return bytes != null ? Deserialize<T>(bytes) : default;
        }
        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            var bytes = Serialize(value);
            return cache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            return cache.RemoveAsync(key, cancellationToken);
        }

        private static T? Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes);
        }

        private static byte[] Serialize<T>(T value)
        {
            var buffer = new ArrayBufferWriter<byte>();
            using (var writer = new Utf8JsonWriter(buffer))
            {
                JsonSerializer.Serialize(writer, value);
                return buffer.WrittenSpan.ToArray();
            }

        }
    }
}
