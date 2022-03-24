using System;

namespace Gurps.Assistant.Domain.Repository.InMemory
{
  using Gurps.Assistant.Domain.Repository.Caching;
  using Gurps.Assistant.Domain.Repository.FetchStrategies;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;

  public abstract class InMemoryRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey>
       where T : class, new()
  {
    private readonly ConcurrentDictionary<TKey, T> items;
    internal InMemoryRepositoryBase(ConcurrentDictionary<TKey, T> items, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(cachingStrategy)
    {
      this.items = items;
    }

    protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
    {
      return CloneDictionary(items).AsQueryable();
    }

    protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
    {
      items.TryGetValue(key, out T result);

      return result;
    }

    private static IEnumerable<T> CloneDictionary(ConcurrentDictionary<TKey, T> list)
    {
      // when you Google deep copy of generic list every answer uses either the IClonable interface on the T or having the T be Serializable
      //  since we can't really put those constraints on T I'm going to do it via reflection

      var type = typeof(T);
      var properties = type.GetProperties();
      var clonedList = new List<T>(list.Count);

      foreach (var keyValuePair in list)
      {
        var newItem = new T();
        foreach (var propInfo in properties)
        {
          // Don't try and set a value to a property w/o a setter
          if (propInfo.CanWrite)
            propInfo.SetValue(newItem, propInfo.GetValue(keyValuePair.Value, null), null);
        }

        clonedList.Add(newItem);
      }

      return clonedList;
    }

    protected override void AddItem(T entity)
    {
      if (GetPrimaryKey(entity, out TKey id) && GenerateKeyOnAdd && Equals(id, default(TKey)))
      {
        id = GeneratePrimaryKey();
        SetPrimaryKey(entity, id);
      }

      items[id] = entity;
    }

    protected override void DeleteItem(T entity)
    {
      GetPrimaryKey(entity, out TKey pkValue);
      items.TryRemove(pkValue, out _);
    }

    protected override void UpdateItem(T entity)
    {
      GetPrimaryKey(entity, out TKey pkValue);

      items[pkValue] = entity;
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public override void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual TKey GeneratePrimaryKey()
    {
      if (typeof(TKey) == typeof(Guid))
      {
        return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
      }

      if (typeof(TKey) == typeof(string))
      {
        return (TKey)Convert.ChangeType(Guid.NewGuid().ToString("N"), typeof(TKey));
      }

      if (typeof(TKey) == typeof(Int32))
      {
        var pkValue = items.Keys.LastOrDefault();

        var nextInt = Convert.ToInt32(pkValue) + 1;
        return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
      }

      if (typeof(TKey) == typeof(Int32))
      {
        var pkValue = items.Keys.LastOrDefault();

        var nextInt = Convert.ToInt32(pkValue) + 1;
        return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
      }

      if (typeof(TKey) == typeof(Int64))
      {
        var pkValue = items.Keys.LastOrDefault();

        var nextInt = Convert.ToInt32(pkValue) + 1;
        return (TKey)Convert.ChangeType(nextInt, typeof(TKey));
      }

      throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
    }

    public override string ToString()
    {
      return "SharpRepository.InMemoryRepository";
    }
  }

}
