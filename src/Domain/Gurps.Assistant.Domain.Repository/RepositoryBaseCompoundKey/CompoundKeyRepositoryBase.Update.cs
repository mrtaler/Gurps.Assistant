using System;
using System.Collections.Generic;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBaseCompoundKey
{
  public abstract partial class CompoundKeyRepositoryBase<T>
  {

    // This is the actual implementation that the derived class needs to implement
    protected abstract void UpdateItem(T entity);

    public void Update(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessUpdate(entity);
    }

    // used from the Update method above and the Save below for the batch save
    private void ProcessUpdate(T entity)
    {
      UpdateItem(entity);

      if (GetPrimaryKeys(entity, out object[] keys))
        _queryManager.OnItemUpdated(keys, entity);
    }

    public void Update(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Update(entity);
      }
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void UpdateItem(T entity);

    public void Update(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessUpdate(entity);
    }

    // used from the Update method above and the Save below for the batch save
    private void ProcessUpdate(T entity)
    {
      UpdateItem(entity);

      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
        _queryManager.OnItemUpdated(key, key2, entity);
    }

    public void Update(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Update(entity);
      }
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void UpdateItem(T entity);

    public void Update(T entity)
    {
      if (entity == null) throw new ArgumentNullException("entity");

      ProcessUpdate(entity);
    }

    // used from the Update method above and the Save below for the batch save
    private void ProcessUpdate(T entity)
    {
      UpdateItem(entity);
      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
        _queryManager.OnItemUpdated(key, key2, key3, entity);
    }

    public void Update(IEnumerable<T> entities)
    {
      if (entities == null) throw new ArgumentNullException("entities");

      foreach (var entity in entities)
      {
        Update(entity);
      }
    }

  }
}
