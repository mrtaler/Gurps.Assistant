using System;
using System.Collections.Generic;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBaseCompoundKey
{
  public abstract partial class CompoundKeyRepositoryBase<T>
  { // This is the actual implementation that the derived class needs to implement
    protected abstract void AddItem(T entity);

    public void Add(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessAdd(entity);
    }

    // used from the Add method above and the Save below for the batch save
    private void ProcessAdd(T entity)
    {
      AddItem(entity);

      if (GetPrimaryKeys(entity, out object[] keys))
        _queryManager.OnItemAdded(keys, entity);
    }

    public void Add(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Add(entity);
      }
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2>
  { // This is the actual implementation that the derived class needs to implement
    protected abstract void AddItem(T entity);

    public void Add(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessAdd(entity);
    }

    // used from the Add method above and the Save below for the batch save
    private void ProcessAdd(T entity)
    {
      AddItem(entity);

      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
        _queryManager.OnItemAdded(key, key2, entity);
    }

    public void Add(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Add(entity);
      }
    }
  }
  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3>
  {
    protected abstract void AddItem(T entity);

    public void Add(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessAdd(entity);
    }

    // used from the Add method above and the Save below for the batch save
    private void ProcessAdd(T entity)
    {
      AddItem(entity);

      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
        _queryManager.OnItemAdded(key, key2, key3, entity);
    }

    public void Add(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Add(entity);
      }
    }
  }
}
