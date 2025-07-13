using System.Buffers;
using System.Text.Json;
using Babylon.Common.Application.Caching;
using Microsoft.Extensions.Caching.Distributed;

namespace Babylon.Common.Infrastructure.Caching;
internal sealed class CacheService(IDistributedCache cache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        byte[]? bytes = await cache.GetAsync(key, cancellationToken);

        return bytes == null ? default : Deserialize<T>(bytes);
    }

    public async Task RemoveAsync<T>(string key, CancellationToken cancellationToken)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration = default, CancellationToken cancellationToken = default)
    {
        byte[] bytes = Serialize(value);

        await cache.SetAsync(key, bytes, CacheOptions.Create(expiration) ,cancellationToken);
    }

    private static T Deserialize<T>(byte[] bytes)
    {
        return JsonSerializer.Deserialize<T>(bytes);
    }
    private static byte[] Serialize<T>(T value)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        JsonSerializer.Serialize(writer, value);
        return buffer.WrittenSpan.ToArray();
    }
}
