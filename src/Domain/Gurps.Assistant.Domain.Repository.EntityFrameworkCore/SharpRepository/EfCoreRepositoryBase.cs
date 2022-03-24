using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.FetchStrategies;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository
{
  public class EfCoreRepositoryBase<T, TKey> : LinqRepositoryBase<T, TKey>
      where T : class
  {
    protected DbSet<T> DbSet { get; private set; }
    private readonly ICoreDbContext context;
    internal EfCoreRepositoryBase(
       ICoreDbContext contextFactory,
        ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(cachingStrategy)
    {
      context = contextFactory;
      DbSet = context.Set<T>();
    }

    protected override void AddItem(T entity)
    {
      if (typeof(TKey) == typeof(Guid) || typeof(TKey) == typeof(string))
      {
        if (GetPrimaryKey(entity, out TKey id) && Equals(id, default(TKey)))
        {
          id = GeneratePrimaryKey();
          SetPrimaryKey(entity, id);
        }
      }
      DbSet.Add(entity);
    }

    protected override void DeleteItem(T entity)
    {
      DbSet.Attach(entity);
      DbSet.Remove(entity);
    }

    protected override void UpdateItem(T entity)
    {
      var entry = context.Entry(entity);

      try
      {
        if (entry.State == EntityState.Detached)
        {

          if (GetPrimaryKey(entity, out TKey key))
          {
            // check to see if this item is already attached
            //  if it is then we need to copy the values to the attached value instead of changing the State to modified since it will throw a duplicate key exception
            //  specifically: "An object with the same key already exists in the ObjectStateManager. The ObjectStateManager cannot track multiple objects with the same key."
            var attachedEntity = context.Set<T>().Find(key);
            if (attachedEntity != null)
            {
              context.Entry(attachedEntity).CurrentValues.SetValues(entity);
              return;
            }
          }
        }
      }
      catch (Exception)
      {
        // ignore and try the default behavior
      }

      // default
      entry.State = EntityState.Modified;
    }

    protected override IQueryable<T> BaseQuery(IFetchStrategy<T> fetchStrategy = null)
    {
      var query = DbSet.AsQueryable();

      return fetchStrategy == null ? query : fetchStrategy.IncludePaths.Aggregate(query, (current, path) => current.Include(path));
    }

    // we override the implementation fro LinqBaseRepository becausee this is built in and doesn't need to find the key column and do dynamic expressions, etc.
    //  this also provides the EF5 first level caching out of the box
    protected override T GetQuery(TKey key, IFetchStrategy<T> fetchStrategy)
    {
      return fetchStrategy == null ? DbSet.Find(key) : base.GetQuery(key, fetchStrategy);
    }

    protected override PropertyInfo GetPrimaryKeyPropertyInfo()
    {
      // checks for the Code First KeyAttribute and if not there do the normal checks
      var type = typeof(T);
      var keyType = typeof(TKey);

      return type.GetProperties().FirstOrDefault(x => x.GetCustomAttributes(typeof(KeyAttribute)).Any() && x.PropertyType == keyType)
          ?? base.GetPrimaryKeyPropertyInfo();
    }

    public override void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing) return;
      if (context == null) return;

      context?.Dispose();
    }

    protected virtual TKey GeneratePrimaryKey()
    {
      if (typeof(TKey) == typeof(Guid))
      {
        return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
      }

      if (typeof(TKey) == typeof(string))
      {
        return (TKey)Convert.ChangeType(Guid.NewGuid().ToString(), typeof(TKey));
      }

      throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
    }
  }
}
