using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.FetchStrategies;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;
using Raven.Client.Documents.Session;

namespace Gurps.Assistant.Domain.Repository.RavenDb.SharpRepository
{
  public class RavenDbRepositoryBase<TEntity, TKey>
      : LinqRepositoryBase<TEntity, TKey>
      where TEntity : class
  {
    public IDocumentSession Session;

    internal RavenDbRepositoryBase(IDocumentSession session,
        ICachingStrategy<TEntity, TKey> cachingStrategy = null)
        : base(cachingStrategy)
    {
      Session = session ?? throw new ArgumentNullException("dbContext");
      /*  Initialize();*/
    }



    private void Initialize()
    {
    }
    /*
    var documentSt = documentStore ?? new DocumentStore { Urls = new string[] { "http://localhost:8080" } };
          documentSt.Initialize();

          //this.DocumentStore = documentStore ?? new DocumentStore { Urls = new string[] { "http://localhost:8080" } };
          // this.DocumentStore.Initialize();

          // see if we need to change the type name that defaults to Id
          var propInfo = this.GetPrimaryKeyPropertyInfo();
          if (propInfo != null && propInfo.Name != "Id")
          {
              // TODO: check this out
              // this is a global convention so will be used regardless of the entity type that is accessing the document store
              //  this may or may not be a problem since the repository creates this and it's for a single entity type
              //  we will need to test this, especially when 2 different repositories are instantiated at the same time
              documentSt.Conventions.FindIdentityProperty = p => p.Name == propInfo.Name;
          }

          // when upgrading to the new RavenDb.Client the following error is thrown when using an int as the PK
          //  System.InvalidOperationException : Attempt to query by id only is blocked, you should use call session.Load("RavenTestIntKeys/1"); instead of session.Query().Where(x=>x.Id == "RavenTestIntKeys/1");
          //      You can turn this error off by specifying documentStore.Conventions.AllowQueriesOnId = true;, but that is not recommend and provided for backward compatibility reasons only.
          //  So for now we will follow that advice and turn on the old convention
          // TODO: look at using a new way of doing the GetQuery to not have this issue when the PK is an int
          // DocumentStore.Conventions.AllowQueriesOnId = true; on 4.0 are supported by default

          //   this.Session = this.DocumentStore.OpenSession();
          this.Session = documentSt.OpenSession();
      }*/

    protected override IQueryable<TEntity> BaseQuery(IFetchStrategy<TEntity> fetchStrategy = null)
    {
      // TODO: see about Raven Include syntax
      return Session.Query<TEntity>();
    }

    protected override TEntity GetQuery(TKey key, IFetchStrategy<TEntity> fetchStrategy)
    {
      try
      {
        return typeof(TKey) == typeof(string) ? Session.Load<TEntity>(key as string) : base.GetQuery(key, fetchStrategy);
      }
      catch (ArgumentException)
      {
        return null;
      }
    }

    public override IEnumerable<TEntity> GetMany(params TKey[] keys)
    {
      return GetMany(keys.ToList());
    }

    public override IEnumerable<TEntity> GetMany(IEnumerable<TKey> keys)
    {
      return keys.Select(Get);
    }

    public override IEnumerable<TResult> GetMany<TResult>(Expression<Func<TEntity, TResult>> selector, params TKey[] keys)
    {
      return GetMany(keys.ToList(), selector);
    }

    public override IEnumerable<TResult> GetMany<TResult>(IEnumerable<TKey> keys, Expression<Func<TEntity, TResult>> selector)
    {
      return keys.Select(x => Get(x, selector));
    }

    public override IDictionary<TKey, TEntity> GetManyAsDictionary(params TKey[] keys)
    {
      return GetManyAsDictionary(keys.ToList());
    }

    public override IDictionary<TKey, TEntity> GetManyAsDictionary(IEnumerable<TKey> keys)
    {
      return GetMany(keys).ToDictionary(GetPrimaryKey);
    }

    public override TResult Min<TResult>(ISpecification<TEntity> criteria, Expression<Func<TEntity, TResult>> selector)
    {
      var pagingOptions = new PagingOptions<TEntity, TResult>(1, 1, selector);

      return QueryManager.ExecuteMin(
          () => FindAll(criteria, selector, pagingOptions).ToList().First(),
          selector,
          criteria
          );
    }

    public override TResult Max<TResult>(ISpecification<TEntity> criteria, Expression<Func<TEntity, TResult>> selector)
    {
      var pagingOptions = new PagingOptions<TEntity, TResult>(1, 1, selector, isDescending: true);

      return QueryManager.ExecuteMin(
          () => FindAll(criteria, selector, pagingOptions).ToList().First(),
          selector,
          criteria
          );
    }

    public override int Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, int>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override decimal? Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, decimal?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override decimal Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, decimal>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double? Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, double?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, double>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override float? Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, float?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override float Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, float>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override int? Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, int?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override long? Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, long?>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override long Sum(ISpecification<TEntity> criteria, Expression<Func<TEntity, long>> selector)
    {
      return QueryManager.ExecuteSum(
           () => FindAll(criteria, selector).ToList().Sum(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, int>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override decimal? Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, decimal?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override decimal Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, decimal>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, double?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, double>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override float? Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, float?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override float Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, float>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, int?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double? Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, long?>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    public override double Average(ISpecification<TEntity> criteria, Expression<Func<TEntity, long>> selector)
    {
      return QueryManager.ExecuteAverage(
           () => FindAll(criteria, selector).ToList().Average(),
           selector,
           criteria
           );
    }

    protected override void AddItem(TEntity entity)
    {
      if (GenerateKeyOnAdd && GetPrimaryKey(entity, out TKey id) && Equals(id, default(TKey)))
      {
        id = GeneratePrimaryKey();
        SetPrimaryKey(entity, id);
      }

      Session.Store(entity);
    }

    protected override void DeleteItem(TEntity entity)
    {
      Session.Delete(entity);
    }

    protected override void UpdateItem(TEntity entity)
    {
      // save changes will take care of it
    }

    public override void Dispose()
    {
      Session?.Dispose();
    }

    public override bool GenerateKeyOnAdd
    {
      get { return base.GenerateKeyOnAdd; }
      set
      {
        if (value == false)
        {
          throw new NotSupportedException("Raven DB driver always generates key values. SharpRepository can't avoid it.");
        }

        base.GenerateKeyOnAdd = value;
      }
    }

    protected virtual TKey GeneratePrimaryKey()
    {
      if (typeof(TKey) == typeof(Guid))
      {
        return (TKey)Convert.ChangeType(Guid.NewGuid(), typeof(TKey));
      }

      if (typeof(TKey) == typeof(int))
      {
        return (TKey)Convert.ChangeType(0, typeof(TKey));
      }

      if (typeof(TKey) == typeof(string))
      {
        // set to the plural of the typename with an ending slash
        //  that means that RavenDB will assign the next ID after the / for us
        //  http://ravendb.net/docs/client-api/basic-operations/saving-new-document
        return (TKey)Convert.ChangeType(TypeName + "s/", typeof(string));
      }

      throw new InvalidOperationException("Primary key could not be generated. This only works for GUID, Int32 and String.");
    }
  }
}