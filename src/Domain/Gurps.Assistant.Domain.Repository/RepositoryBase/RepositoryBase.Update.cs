using System;
using System.Collections.Generic;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBase
{
  public abstract partial class RepositoryBase<T, TKey>
  {
    // This is the actual implementation that the derived class needs to implement
    protected abstract void UpdateItem(T entity);

    public void Update(T entity)
    {
      try
      {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        ProcessUpdate(entity/*, BatchMode*/);
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }

    // used from the Update method above and the Save below for the batch save
    private void ProcessUpdate(T entity/*, bool batchMode*/)
    {
      if (!RunAspect(attribute => attribute.OnUpdateExecuting(entity, _repositoryActionContext)))
        return;

      UpdateItem(entity);

      RunAspect(attribute => attribute.OnUpdateExecuted(entity, _repositoryActionContext));

      /* if (batchMode) return;

       Save();*/

      NotifyQueryManagerOfUpdatedEntity(entity);
    }

    private void NotifyQueryManagerOfUpdatedEntity(T entity)
    {
      if (GetPrimaryKey(entity, out TKey key))
        QueryManager.OnItemUpdated(key, entity);
    }

    public void Update(IEnumerable<T> entities)
    {
      try
      {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        //using (var batch = BeginBatch())
        //{
        foreach (var entity in entities)
        {
          // batch.
          Update(entity);
        }

        //    batch.Commit();
        //}
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }
  }
}
