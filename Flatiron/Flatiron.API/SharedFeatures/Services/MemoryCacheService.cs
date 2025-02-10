namespace Flatiron.API.SharedFeatures.Services;

public class MemoryCacheService<T> : ICacheService<T>
{
    private readonly IMemoryCache _memoryCache;
    readonly Guid _key = Guid.NewGuid();

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void Set(T data)
    {
        var expiration = TimeSpan.FromDays(1);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expiration);

        _memoryCache.Set(_key, data, cacheEntryOptions);
    }

    public T? Get()
    {
        _memoryCache.TryGetValue(_key, out T? data);
        return data;
    }

    public void Remove()
    {
        _memoryCache.Remove(_key);
    }
}

