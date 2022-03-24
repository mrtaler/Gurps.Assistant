using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBase
{
  public abstract partial class RepositoryBase<T, TKey>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void DeleteItem(T entity);

    public void Delete(T entity)
    {
      try
      {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        ProcessDelete(entity/*, BatchMode*/);
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }

    // used from the Delete method above and the Save below for the batch save
    private void ProcessDelete(T entity/*, bool batchMode*/)
    {
      if (!RunAspect(attribute => attribute.OnDeleteExecuting(entity, _repositoryActionContext)))
        return;

      DeleteItem(entity);

      RunAspect(attribute => attribute.OnDeleteExecuted(entity, _repositoryActionContext));

      // if (batchMode) return;
      // Save();

      NotifyQueryManagerOfDeletedEntity(entity);
    }

    private void NotifyQueryManagerOfDeletedEntity(T entity)
    {
      if (GetPrimaryKey(entity, out TKey key))
        QueryManager.OnItemDeleted(key, entity);
    }

    public void Delete(IEnumerable<T> entities)
    {
      foreach (var entity in entities)
      {
        Delete(entity);
      }
    }

    public void Delete(IEnumerable<TKey> keys)
    {
      Delete(keys.ToArray());
    }

    public void Delete(params TKey[] keys)
    {
      try
      {
        //using (var batch = BeginBatch())
        //{
        foreach (var key in keys)
        {
          var entity = Get(key);

          if (entity == null) throw new ArgumentException("No entity exists with this key.", "key");

          // batch.
          Delete(entity);
        }

        //batch.Commit();
        //}
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }

    public void Delete(TKey key)
    {
      try
      {
        var entity = Get(key);

        if (entity == null) throw new ArgumentException("No entity exists with this key.", nameof(key));

        Delete(entity);
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }

    public void Delete(Expression<Func<T, bool>> predicate)
    {
      Delete(CreateSpecification(predicate));
    }

    public void Delete(ISpecification<T> criteria)
    {
      Delete(FindAll(criteria));
    }
  }
}
