using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBaseCompoundKey
{
  public abstract partial class CompoundKeyRepositoryBase<T>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void DeleteItem(T entity);

    public void Delete(T entity)
    {
      if (entity == null) throw new ArgumentNullException(nameof(entity));

      ProcessDelete(entity);
    }

    // used from the Delete method above and the Save below for the batch save
    private void ProcessDelete(T entity)
    {
      DeleteItem(entity);


      if (GetPrimaryKeys(entity, out object[] keys))
        _queryManager.OnItemDeleted(keys, entity);
    }

    public void Delete(IEnumerable<T> entities)
    {
      foreach (var entity in entities)
      {
        Delete(entity);
      }
    }

    public void Delete(params object[] keys)
    {
      var entity = Get(keys);

      if (entity == null) throw new ArgumentException("No entity exists with these keys.", nameof(keys));

      Delete(entity);
    }

    public void Delete(Expression<Func<T, bool>> predicate)
    {
      Delete(new Specification<T>(predicate));
    }

    public void Delete(ISpecification<T> criteria)
    {
      Delete(FindAll(criteria));
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void DeleteItem(T entity);

    public void Delete(T entity)
    {
      if (entity == null) throw new ArgumentNullException(nameof(entity));

      ProcessDelete(entity);
    }

    // used from the Delete method above and the Save below for the batch save
    private void ProcessDelete(T entity)
    {
      DeleteItem(entity);

      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2))
        _queryManager.OnItemDeleted(key, key2, entity);
    }

    public void Delete(IEnumerable<T> entities)
    {
      foreach (var entity in entities)
      {
        Delete(entity);
      }
    }

    public void Delete(TKey key, TKey2 key2)
    {
      var entity = Get(key, key2);

      if (entity == null) throw new ArgumentException("No entity exists with this key.", nameof(key));

      Delete(entity);
    }

    public void Delete(Expression<Func<T, bool>> predicate)
    {
      Delete(new Specification<T>(predicate));
    }

    public void Delete(ISpecification<T> criteria)
    {
      Delete(FindAll(criteria));
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void DeleteItem(T entity);

    public void Delete(T entity)
    {
      if (entity == null) throw new ArgumentNullException(nameof(entity));

      ProcessDelete(entity);
    }

    // used from the Delete method above and the Save below for the batch save
    private void ProcessDelete(T entity)
    {
      DeleteItem(entity);

      if (GetPrimaryKey(entity, out TKey key, out TKey2 key2, out TKey3 key3))
        _queryManager.OnItemDeleted(key, key2, key3, entity);
    }

    public void Delete(IEnumerable<T> entities)
    {
      foreach (var entity in entities)
      {
        Delete(entity);
      }
    }

    public void Delete(TKey key, TKey2 key2, TKey3 key3)
    {
      var entity = Get(key, key2, key3);

      if (entity == null) throw new ArgumentException("No entity exists with these keys.", nameof(key));

      Delete(entity);
    }

    public void Delete(Expression<Func<T, bool>> predicate)
    {
      Delete(new Specification<T>(predicate));
    }

    public void Delete(ISpecification<T> criteria)
    {
      Delete(FindAll(criteria));
    }
  }
}
