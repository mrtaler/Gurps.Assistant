using System;
using Microsoft.Extensions.Caching.Memory;

namespace Gurps.Assistant.Domain.Repository.Caching
{
  /// <summary>
  /// Uses the .NET built-in MemoryCache as the caching provider.
  /// </summary>
  public class InMemoryCachingProvider : ICachingProvider
  {
    protected IMemoryCache Cache;
    public InMemoryCachingProvider(IMemoryCache cache)
    {
      Cache = cache;
    }

    private static readonly object LockObject = new();

    public void Set<T>(string key, T value, CacheItemPriority priority = CacheItemPriority.Normal, int? cacheTime = null)
    {
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

      var policy = new MemoryCacheEntryOptions()
      {
        Priority = priority
      };
      if (cacheTime.HasValue)
      {
        policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime.Value);
      }

      Cache.Set(key, value, policy);
    }

    public void Clear(string key)
    {
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

      Cache.Remove(key);
    }

    public bool Exists(string key)
    {
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
      return Cache.TryGetValue(key, out _);
    }

    public bool Get<T>(string key, out T value)
    {
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

      value = default;

      try
      {
        if (!Exists(key))
          return false;

        value = Cache.Get<T>(key);
      }
      catch (Exception)
      {
        // ignore and use default
        return false;
      }

      return true;
    }

    public int Increment(string key, int defaultValue, int incrementValue, CacheItemPriority priority = CacheItemPriority.Normal)
    {
      if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

      lock (LockObject)
      {
        if (!Get(key, out int current))
        {
          current = defaultValue;
        }

        var newValue = current + incrementValue;
        Set(key, newValue, priority);
        return newValue;
      }
    }

    protected virtual void Dispose(bool disposing) => Cache = new MemoryCache(new MemoryCacheOptions());

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }
  }
}
