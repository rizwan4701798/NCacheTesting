using Alachisoft.NCache.Client;

namespace NCacheTesting.Services;

public class CacheService
{
    private readonly ICache _cache;

    public CacheService(ICache cache)
    {
        _cache = cache;
    }

    public void Add<T>(string key, T item)
    {
        var cacheItem = new CacheItem(item);
        _cache.Add(key, cacheItem);
        Console.WriteLine($"Item added to cache: {key}");
    }

    public T? Get<T>(string key)
    {
        var item = _cache.Get<T>(key);
        if (item == null)
        {
            Console.WriteLine($"Item not found in cache: {key}");
        }
        else
        {
            Console.WriteLine($"Item retrieved from cache: {key}");
        }
        return item;
    }

    public void Update<T>(string key, T item)
    {
        var cacheItem = new CacheItem(item);
        _cache.Insert(key, cacheItem);
        Console.WriteLine($"Item updated in cache: {key}");
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        Console.WriteLine($"Item removed from cache: {key}");
    }

    public bool Exists(string key)
    {
        return _cache.Contains(key);
    }

    public void AddBulk<T>(IDictionary<string, T> items)
    {
        var cacheItems = new Dictionary<string, CacheItem>();
        foreach (var item in items)
        {
            cacheItems.Add(item.Key, new CacheItem(item.Value));
        }

        var results = _cache.AddBulk(cacheItems);
        foreach (var result in results)
        {
            if (result.Value is Exception ex)
            {
                Console.WriteLine($"Failed to add {result.Key}: {ex.Message}");
            }
            else
            {
                Console.WriteLine($"Item added to cache: {result.Key}");
            }
        }
    }

    public IDictionary<string, T> GetBulk<T>(IEnumerable<string> keys)
    {
        var result = _cache.GetBulk<T>(keys);
        Console.WriteLine($"Retrieved {result.Count} items from cache.");
        return result;
    }

    public void UpdateBulk<T>(IDictionary<string, T> items)
    {
        var cacheItems = new Dictionary<string, CacheItem>();
        foreach (var item in items)
        {
            cacheItems.Add(item.Key, new CacheItem(item.Value));
        }

        var results = _cache.InsertBulk(cacheItems);
        foreach (var result in results)
        {
             if (result.Value is Exception ex)
            {
                Console.WriteLine($"Failed to update {result.Key}: {ex.Message}");
            }
            else
            {
                Console.WriteLine($"Item updated in cache: {result.Key}");
            }
        }
    }

    public void RemoveBulk(IEnumerable<string> keys)
    {
        _cache.RemoveBulk(keys);
        Console.WriteLine($"Items removed from cache: {string.Join(", ", keys)}");
    }
}
