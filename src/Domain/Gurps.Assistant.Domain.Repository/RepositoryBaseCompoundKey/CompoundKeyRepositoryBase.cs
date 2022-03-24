using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Gurps.Assistant.Domain.Repository.Attributes;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.FetchStrategies;
using Gurps.Assistant.Domain.Repository.Helpers;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace RolePlayedGamesHelper.Repository.SharpRepository.RepositoryBaseCompoundKey
{
  public abstract partial class CompoundKeyRepositoryBase<T> : ICompoundKeyRepository<T>
        where T : class
  {
    // the caching strategy used
    private ICompoundKeyCachingStrategy<T> _cachingStrategy;

    // the query manager uses the caching strategy to determine if it should check the cache or run the query
    private CompoundKeyQueryManager<T> _queryManager;

    // just the type name, used to find the primary key if it is [TypeName]Id
    private readonly string _typeName;
    protected string TypeName
    {
      get { return _typeName; }
    }

    public bool CacheUsed
    {
      get { return _queryManager.CacheUsed; }
    }

    protected CompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T> cachingStrategy = null)
    {
      CachingStrategy = cachingStrategy ?? new NoCompoundKeyCachingStrategy<T>();
      _typeName = typeof(T).Name;
    }

    public ICompoundKeyCachingStrategy<T> CachingStrategy
    {
      get { return _cachingStrategy; }
      set
      {
        _cachingStrategy = value ?? new NoCompoundKeyCachingStrategy<T>();
        _queryManager = new CompoundKeyQueryManager<T>(_cachingStrategy);
      }
    }

    public abstract IQueryable<T> AsQueryable();

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy);
    protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

    public IEnumerable<T> GetAll()
    {
      return GetAll((IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
    {
      return GetAll((IQueryOptions<T>)null, fetchStrategy);
    }

    public IEnumerable<T> GetAll(params string[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
    {
      return GetAll(queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
          null,
          queryOptions
          );
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
    {
      return GetAll(selector, (IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
    {
      return GetAll(selector, queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
    {
      return GetAll(selector, null, fetchStrategy);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).Select(selectFunc).ToList(),
          selector,
          queryOptions
          );
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        where TInner : class
        where TResult : class;

    // These are the actual implementation that the derived class needs to implement
    protected abstract T GetQuery(params object[] keys);

    public T Get(params object[] keys)
    {
      return _queryManager.ExecuteGet(
          () => GetQuery(keys),
          null,
          keys
          );
    }

    public TResult Get<TResult>(Expression<Func<T, TResult>> selector, params object[] keys)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGet(
          () =>
          {
            var result = GetQuery(keys);
            if (result == null)
              return default;

            var results = new[] { result };
            return results.AsEnumerable().Select(selectFunc).First();
          },
          selector,
          keys
          );
    }

    public bool Exists(params object[] keys)
    {
      return TryGet(out _, keys);
    }

    public bool TryGet(out T entity, params object[] keys)
    {
      entity = default;

      try
      {
        entity = Get(keys);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria);
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).ToList(),
          criteria,
          null,
          queryOptions
          );
    }

    public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).Select(selectFunc).ToList(),
          criteria,
          selector,
          queryOptions
          );
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return FindAll(new Specification<T>(predicate), queryOptions);
    }

    public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return FindAll(new Specification<T>(predicate), selector, queryOptions);
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract T FindQuery(ISpecification<T> criteria);
    protected abstract T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFind(
          () => FindQuery(criteria, queryOptions),
          criteria,
          null,
          null
          );
    }

    public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFind(
          () =>
          {
            var result = FindQuery(criteria, queryOptions);
            if (result == null)
              return default;

            var results = new[] { result };
            return results.AsEnumerable().Select(selectFunc).First();
          },
          criteria,
          selector,
          null
          );
    }

    public bool Exists(ISpecification<T> criteria)
    {
      return TryFind(criteria, out _);
    }

    public bool TryFind(ISpecification<T> criteria, out T entity)
    {
      return TryFind(criteria, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(criteria, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(criteria, selector, null, out entity);
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(criteria, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return Find(new Specification<T>(predicate), queryOptions);
    }

    public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return Find(new Specification<T>(predicate), selector, queryOptions);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
      return TryFind(predicate, out _);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
    {
      return TryFind(predicate, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(predicate, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(predicate, selector, null, out entity);
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(predicate, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    //private void Save()
    //{
    //    SaveChanges();

    //    _queryManager.OnSaveExecuted();
    //}


    public abstract void Dispose();

    protected virtual bool GetPrimaryKeys(T entity, out object[] keys)
    {
      keys = null;
      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null)
        return false;

      keys = propInfo.Select(info => info.GetValue(entity, null)).ToArray();

      return true;
    }

    protected virtual bool SetPrimaryKey(T entity, object[] keys)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null || keys == null || propInfo.Length != keys.Length)
        return false;

      var i = 0;
      foreach (var key in keys)
      {
        propInfo[i].SetValue(entity, key, null);
        i++;
      }

      return true;
    }

    protected virtual ISpecification<T> ByPrimaryKeySpecification(object[] keys)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();
      if (propInfo == null || keys == null || propInfo.Length != keys.Length)
        return null;

      ISpecification<T> specification = null;
      var parameter = Expression.Parameter(typeof(T), "x");

      var i = 0;
      foreach (var lambda in keys.Select(key => Expression.Lambda<Func<T, bool>>(
                  Expression.Equal(
                      Expression.PropertyOrField(parameter, propInfo[i].Name),
                      Expression.Constant(key)
                  ),
                  parameter
              ))
          )
      {
        specification = specification == null ? new Specification<T>(lambda) : specification.And(lambda);
        i++;
      }

      return specification;
    }

    protected virtual PropertyInfo[] GetPrimaryKeyPropertyInfo()
    {
      var type = typeof(T);
      var properties = type.GetTypeInfo().DeclaredProperties;

      return properties.Where(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>()).OrderBy(x => x.GetOneAttribute<RepositoryPrimaryKeyAttribute>().Order).ToArray();
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2> : ICompoundKeyRepository<T, TKey, TKey2>
      where T : class
  {
    // the caching strategy used
    private ICompoundKeyCachingStrategy<T, TKey, TKey2> _cachingStrategy;

    // the query manager uses the caching strategy to determine if it should check the cache or run the query
    private CompoundKeyQueryManager<T, TKey, TKey2> _queryManager;

    // just the type name, used to find the primary key if it is [TypeName]Id
    private readonly string _typeName;
    protected string TypeName
    {
      get { return _typeName; }
    }

    public bool CacheUsed
    {
      get { return _queryManager.CacheUsed; }
    }
    protected CompoundKeyRepositoryBase(
        ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
    {
      if (typeof(T) == typeof(TKey))
      {
        // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
        throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
      }

      CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2>();
      _typeName = typeof(T).Name;
    }

    public ICompoundKeyCachingStrategy<T, TKey, TKey2> CachingStrategy
    {
      get { return _cachingStrategy; }
      set
      {
        _cachingStrategy = value ?? new NoCachingStrategy<T, TKey, TKey2>();
        _queryManager = new CompoundKeyQueryManager<T, TKey, TKey2>(_cachingStrategy);
      }
    }

    public abstract IQueryable<T> AsQueryable();

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy);
    protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

    public IEnumerable<T> GetAll()
    {
      return GetAll((IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
    {
      return GetAll((IQueryOptions<T>)null, fetchStrategy);
    }

    public IEnumerable<T> GetAll(params string[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
    {
      return GetAll(queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
          null,
          queryOptions
          );
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
    {
      return GetAll(selector, (IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
    {
      return GetAll(selector, queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
    {
      return GetAll(selector, null, fetchStrategy);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).Select(selectFunc).ToList(),
          selector,
          queryOptions
          );
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract T GetQuery(TKey key, TKey2 key2);

    public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        where TInner : class
        where TResult : class;

    public T Get(TKey key, TKey2 key2)
    {
      return _queryManager.ExecuteGet(
          () => GetQuery(key, key2),
          null,
          key,
          key2
          );
    }

    public TResult Get<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGet(
          () =>
          {
            var result = GetQuery(key, key2);
            if (result == null)
              return default;

            var results = new[] { result };
            return results.AsEnumerable().Select(selectFunc).First();
          },
          selector,
          key,
          key2
          );
    }

    public bool Exists(TKey key, TKey2 key2)
    {
      return TryGet(key, key2, out _);
    }

    public bool TryGet(TKey key, TKey2 key2, out T entity)
    {
      entity = default;

      try
      {
        entity = Get(key, key2);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryGet<TResult>(TKey key, TKey2 key2, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Get(key, key2, selector);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria);
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).ToList(),
          criteria,
          null,
          queryOptions
          );
    }

    public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).Select(selectFunc).ToList(),
          criteria,
          selector,
          queryOptions
          );
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return FindAll(new Specification<T>(predicate), queryOptions);
    }

    public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return FindAll(new Specification<T>(predicate), selector, queryOptions);
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract T FindQuery(ISpecification<T> criteria);
    protected abstract T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFind(
          () => FindQuery(criteria, queryOptions),
          criteria,
          null,
          null
          );
    }

    public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFind(
          () =>
              {
                var result = FindQuery(criteria, queryOptions);
                if (result == null)
                  return default;

                var results = new[] { result };
                return results.AsEnumerable().Select(selectFunc).First();
              },
          criteria,
          selector,
          null
          );
    }

    public bool Exists(ISpecification<T> criteria)
    {
      return TryFind(criteria, out _);
    }

    public bool TryFind(ISpecification<T> criteria, out T entity)
    {
      return TryFind(criteria, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(criteria, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(criteria, selector, null, out entity);
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(criteria, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return Find(new Specification<T>(predicate), queryOptions);
    }

    public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return Find(new Specification<T>(predicate), selector, queryOptions);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
      return TryFind(predicate, out _);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
    {
      return TryFind(predicate, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(predicate, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(predicate, selector, null, out entity);
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(predicate, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    public abstract void Dispose();

    protected virtual bool GetPrimaryKey(T entity, out TKey key, out TKey2 key2)
    {
      key = default;
      key2 = default;

      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null || propInfo.Length != 2)
        return false;

      key = (TKey)propInfo[0].GetValue(entity, null);
      key2 = (TKey2)propInfo[1].GetValue(entity, null);

      return true;
    }

    protected virtual bool SetPrimaryKey(T entity, TKey key, TKey2 key2)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null || propInfo.Length != 2)
        return false;

      propInfo[0].SetValue(entity, key, null);
      propInfo[1].SetValue(entity, key2, null);

      return true;
    }

    protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key, TKey2 key2)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();
      if (propInfo == null || propInfo.Length != 2)
        return null;

      var parameter = Expression.Parameter(typeof(T), "x");
      var lambda = Expression.Lambda<Func<T, bool>>(
              Expression.Equal(
                  Expression.PropertyOrField(parameter, propInfo[0].Name),
                  Expression.Constant(key)
              ),
              parameter
          );
      var lambda2 = Expression.Lambda<Func<T, bool>>(
              Expression.Equal(
                  Expression.PropertyOrField(parameter, propInfo[1].Name),
                  Expression.Constant(key2)
              ),
              parameter
          );

      return new Specification<T>(lambda).And(lambda2);
    }

    protected virtual PropertyInfo[] GetPrimaryKeyPropertyInfo()
    {
      var type = typeof(T);
      var properties = type.GetTypeInfo().DeclaredProperties;

      return properties.Where(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>()).OrderBy(x => x.GetOneAttribute<RepositoryPrimaryKeyAttribute>().Order).ToArray();
    }
  }

  public abstract partial class CompoundKeyRepositoryBase<T, TKey, TKey2, TKey3> : ICompoundKeyRepository<T, TKey, TKey2, TKey3>
      where T : class
  {
    // the caching strategy used
    private ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> _cachingStrategy;

    // the query manager uses the caching strategy to determine if it should check the cache or run the query
    private CompoundKeyQueryManager<T, TKey, TKey2, TKey3> _queryManager;

    // just the type name, used to find the primary key if it is [TypeName]Id
    private readonly string _typeName;
    protected string TypeName
    {
      get { return _typeName; }
    }

    public bool CacheUsed
    {
      get { return _queryManager.CacheUsed; }
    }


    protected CompoundKeyRepositoryBase(ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
    {
      if (typeof(T) == typeof(TKey))
      {
        // this check is mainly because of the overloaded Delete methods Delete(T) and Delete(TKey), ambiguous reference if the generics are the same
        throw new InvalidOperationException("The repository type and the primary key type can not be the same.");
      }

      CachingStrategy = cachingStrategy ?? new NoCachingStrategy<T, TKey, TKey2, TKey3>();
      _typeName = typeof(T).Name;
    }

    public ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> CachingStrategy
    {
      get { return _cachingStrategy; }
      set
      {
        _cachingStrategy = value ?? new NoCachingStrategy<T, TKey, TKey2, TKey3>();
        _queryManager = new CompoundKeyQueryManager<T, TKey, TKey2, TKey3>(_cachingStrategy);
      }
    }

    public abstract IQueryable<T> AsQueryable();

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> GetAllQuery(IFetchStrategy<T> fetchStrategy);
    protected abstract IQueryable<T> GetAllQuery(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy);

    public IEnumerable<T> GetAll()
    {
      return GetAll((IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IFetchStrategy<T> fetchStrategy)
    {
      return GetAll((IQueryOptions<T>)null, fetchStrategy);
    }

    public IEnumerable<T> GetAll(params string[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions)
    {
      return GetAll(queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).ToList(),
          null,
          queryOptions
          );
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<T> GetAll(IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector)
    {
      return GetAll(selector, (IQueryOptions<T>)null, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions)
    {
      return GetAll(selector, queryOptions, (IFetchStrategy<T>)null);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IFetchStrategy<T> fetchStrategy)
    {
      return GetAll(selector, null, fetchStrategy);
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params string[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, IFetchStrategy<T> fetchStrategy)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGetAll(
          () => GetAllQuery(queryOptions, fetchStrategy).Select(selectFunc).ToList(),
          selector,
          queryOptions
          );
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params string[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy<T>(includePaths));
    }

    public IEnumerable<TResult> GetAll<TResult>(Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, params Expression<Func<T, object>>[] includePaths)
    {
      return GetAll(selector, queryOptions, RepositoryHelper.BuildFetchStrategy(includePaths));
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract T GetQuery(TKey key, TKey2 key2, TKey3 key3);

    public abstract IRepositoryQueryable<TResult> Join<TJoinKey, TInner, TResult>(IRepositoryQueryable<TInner> innerRepository, Expression<Func<T, TJoinKey>> outerKeySelector, Expression<Func<TInner, TJoinKey>> innerKeySelector, Expression<Func<T, TInner, TResult>> resultSelector)
        where TInner : class
        where TResult : class;

    public T Get(TKey key, TKey2 key2, TKey3 key3)
    {
      return _queryManager.ExecuteGet(
          () => GetQuery(key, key2, key3),
          null,
          key,
          key2,
          key3
          );
    }

    public TResult Get<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector)
    {
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteGet(
          () =>
          {
            var result = GetQuery(key, key2, key3);
            if (result == null)
              return default;

            var results = new[] { result };
            return results.AsEnumerable().Select(selectFunc).First();
          },
          selector,
          key,
          key2,
          key3
          );
    }

    public bool Exists(TKey key, TKey2 key2, TKey3 key3)
    {
      return TryGet(key, key2, key3, out _);
    }

    public bool TryGet(TKey key, TKey2 key2, TKey3 key3, out T entity)
    {
      entity = default;

      try
      {
        entity = Get(key, key2, key3);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryGet<TResult>(TKey key, TKey2 key2, TKey3 key3, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Get(key, key2, key3, selector);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria);
    protected abstract IQueryable<T> FindAllQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public IEnumerable<T> FindAll(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).ToList(),
          criteria,
          null,
          queryOptions
          );
    }

    public IEnumerable<TResult> FindAll<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFindAll(
          () => FindAllQuery(criteria, queryOptions).Select(selectFunc).ToList(),
          criteria,
          selector,
          queryOptions
          );
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return FindAll(new Specification<T>(predicate), queryOptions);
    }

    public IEnumerable<TResult> FindAll<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return FindAll(new Specification<T>(predicate), selector, queryOptions);
    }

    // These are the actual implementation that the derived class needs to implement
    protected abstract T FindQuery(ISpecification<T> criteria);
    protected abstract T FindQuery(ISpecification<T> criteria, IQueryOptions<T> queryOptions);

    public T Find(ISpecification<T> criteria, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));

      return _queryManager.ExecuteFind(
          () => FindQuery(criteria, queryOptions),
          criteria,
          null,
          null
          );
    }

    public TResult Find<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (criteria == null) throw new ArgumentNullException(nameof(criteria));
      if (selector == null) throw new ArgumentNullException(nameof(selector));
      var selectFunc = selector.Compile();

      return _queryManager.ExecuteFind(
          () =>
              {
                var result = FindQuery(criteria, queryOptions);
                if (result == null)
                  return default;

                var results = new[] { result };
                return results.AsEnumerable().Select(selectFunc).First();
              },
          criteria,
          selector,
          null
          );
    }

    public bool Exists(ISpecification<T> criteria)
    {
      return TryFind(criteria, out _);
    }

    public bool TryFind(ISpecification<T> criteria, out T entity)
    {
      return TryFind(criteria, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(ISpecification<T> criteria, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(criteria, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(criteria, selector, null, out entity);
    }

    public bool TryFind<TResult>(ISpecification<T> criteria, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(criteria, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    public T Find(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));

      return Find(new Specification<T>(predicate), queryOptions);
    }

    public TResult Find<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions = null)
    {
      if (predicate == null) throw new ArgumentNullException(nameof(predicate));
      if (selector == null) throw new ArgumentNullException(nameof(selector));

      return Find(new Specification<T>(predicate), selector, queryOptions);
    }

    public bool Exists(Expression<Func<T, bool>> predicate)
    {
      return TryFind(predicate, out _);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, out T entity)
    {
      return TryFind(predicate, (IQueryOptions<T>)null, out entity);
    }

    public bool TryFind(Expression<Func<T, bool>> predicate, IQueryOptions<T> queryOptions, out T entity)
    {
      entity = null;

      try
      {
        entity = Find(predicate, queryOptions);
        return entity != null;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, out TResult entity)
    {
      return TryFind(predicate, selector, null, out entity);
    }

    public bool TryFind<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, IQueryOptions<T> queryOptions, out TResult entity)
    {
      entity = default;

      try
      {
        entity = Find(predicate, selector, queryOptions);
        return !entity.Equals(default(TResult));
      }
      catch (Exception)
      {
        return false;
      }
    }

    // This is the actual implementation that the derived class needs to implement

    public abstract void Dispose();

    protected virtual bool GetPrimaryKey(T entity, out TKey key, out TKey2 key2, out TKey3 key3)
    {
      key = default;
      key2 = default;
      key3 = default;

      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null || propInfo.Length != 3)
        return false;

      key = (TKey)propInfo[0].GetValue(entity, null);
      key2 = (TKey2)propInfo[1].GetValue(entity, null);
      key3 = (TKey3)propInfo[2].GetValue(entity, null);

      return true;
    }

    protected virtual bool SetPrimaryKey(T entity, TKey key, TKey2 key2, TKey3 key3)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();

      // if there is no property that matches then return false
      if (propInfo == null || propInfo.Length != 3)
        return false;

      propInfo[0].SetValue(entity, key, null);
      propInfo[1].SetValue(entity, key2, null);
      propInfo[2].SetValue(entity, key3, null);

      return true;
    }

    protected virtual ISpecification<T> ByPrimaryKeySpecification(TKey key, TKey2 key2, TKey3 key3)
    {
      var propInfo = GetPrimaryKeyPropertyInfo();
      if (propInfo == null || propInfo.Length != 3)
        return null;

      var parameter = Expression.Parameter(typeof(T), "x");
      var lambda = Expression.Lambda<Func<T, bool>>(
              Expression.Equal(
                  Expression.PropertyOrField(parameter, propInfo[0].Name),
                  Expression.Constant(key)
              ),
              parameter
          );
      var lambda2 = Expression.Lambda<Func<T, bool>>(
              Expression.Equal(
                  Expression.PropertyOrField(parameter, propInfo[1].Name),
                  Expression.Constant(key2)
              ),
              parameter
          );
      var lambda3 = Expression.Lambda<Func<T, bool>>(
              Expression.Equal(
                  Expression.PropertyOrField(parameter, propInfo[2].Name),
                  Expression.Constant(key3)
              ),
              parameter
          );

      return new Specification<T>(lambda).And(lambda2).And(lambda3);
    }

    protected virtual PropertyInfo[] GetPrimaryKeyPropertyInfo()
    {
      var type = typeof(T);
      var properties = type.GetTypeInfo().DeclaredProperties;
      return properties.Where(x => x.HasAttribute<RepositoryPrimaryKeyAttribute>()).OrderBy(x => x.GetOneAttribute<RepositoryPrimaryKeyAttribute>().Order).ToArray();
    }
  }
}
