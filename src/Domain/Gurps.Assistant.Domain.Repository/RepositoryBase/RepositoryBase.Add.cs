using System;
using System.Collections.Generic;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBase
{
  public abstract partial class RepositoryBase<T, TKey>
  { // This is the actual implementation that the derived class needs to implement
    protected abstract void AddItem(T entity);

    public void Add(T entity)
    {
      try
      {
        if (entity == null) throw new ArgumentNullException("entity");

        ProcessAdd(entity/*, BatchMode*/);
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }

    // used from the Add method above and the Save below for the batch save
    private void ProcessAdd(T entity/*, bool batchMode*/)
    {
      if (!RunAspect(attribute => attribute.OnAddExecuting(entity, _repositoryActionContext)))
        return;

      AddItem(entity);

      RunAspect(attribute => attribute.OnAddExecuted(entity, _repositoryActionContext));

      // if (batchMode) return;
      // Save();

      NotifyQueryManagerOfAddedEntity(entity);
    }

    private void NotifyQueryManagerOfAddedEntity(T entity)
    {
      if (GetPrimaryKey(entity, out TKey key))
        QueryManager.OnItemAdded(key, entity);
    }

    public void Add(IEnumerable<T> entities)
    {
      try
      {
        if (entities == null) throw new ArgumentNullException("entities");

        // using (var batch = BeginBatch())
        //  {
        foreach (var entity in entities)
        {
          /*batch.*/
          Add(entity);
        }

        //   batch.Commit();
        //  }
      }
      catch (Exception ex)
      {
        Error(ex);
        throw;
      }
    }
  }
}
