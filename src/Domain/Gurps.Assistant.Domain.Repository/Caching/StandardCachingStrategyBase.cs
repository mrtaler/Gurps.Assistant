using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Gurps.Assistant.Domain.Repository.Helpers;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;
using Microsoft.Extensions.Caching.Memory;

// References that were helpful in developing the Write Through Caching and Generational Caching logic
//  http://www.regexprn.com/2011/06/web-application-caching-strategies.html
//  http://www.regexprn.com/2011/06/web-application-caching-strategies_05.html
//  http://37signals.com/svn/posts/3113-how-key-based-cache-expiration-works
//  http://assets.en.oreilly.com/1/event/27/Accelerate%20your%20Rails%20Site%20with%20Automatic%20Generation-based%20Action%20Caching%20Presentation%201.pdf

namespace Gurps.Assistant.Domain.Repository.Caching
{
  public abstract class StandardCachingStrategyBase<T, TKey, TPartition> : CachingStrategyBase<T, TKey> where T : class
  {
    public bool WriteThroughCachingEnabled { get; set; }
    public bool GenerationalCachingEnabled { get; set; }
    public Expression<Func<T, TPartition>> Partition { get; set; }

    internal StandardCachingStrategyBase(int? maxResults)
        : base(maxResults)
    {
      WriteThroughCachingEnabled = true;
      GenerationalCachingEnabled = true;
      Partition = null;
    }

    internal StandardCachingStrategyBase(int? maxResults, ICachingProvider cachingProvider)
        : base(maxResults, cachingProvider)
    {
      WriteThroughCachingEnabled = true;
      GenerationalCachingEnabled = true;
      Partition = null;
    }

    public override bool TryGetResult(TKey key, out T result)
    {
      result = default;

      if (!WriteThroughCachingEnabled)
        return false;

      return base.TryGetResult(key, out result);
    }

    public override void SaveGetResult(TKey key, T result)
    {
      if (!WriteThroughCachingEnabled) return;

      base.SaveGetResult(key, result);
    }

    public override bool TryGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
    {
      result = null;

      if (!GenerationalCachingEnabled) return false;

      return base.TryGetAllResult(queryOptions, selector, out result);
    }

    public override void SaveGetAllResult<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
    {
      if (!GenerationalCachingEnabled) return;
      if (MaxResults.HasValue && result.Count() > MaxResults.Value) return;

      base.SaveGetAllResult(queryOptions, selector, result);
    }

    public override bool TryFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out IEnumerable<TResult> result)
    {
      result = null;

      if (!GenerationalCachingEnabled) return false;

      return base.TryFindAllResult(criteria, queryOptions, selector, out result);
    }

    public override void SaveFindAllResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, IEnumerable<TResult> result)
    {
      if (!GenerationalCachingEnabled) return;
      if (MaxResults.HasValue && result.Count() > MaxResults.Value) return;

      base.SaveFindAllResult(criteria, queryOptions, selector, result);
    }

    public override bool TryFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, out TResult result)
    {
      result = default;

      if (!GenerationalCachingEnabled) return false;

      return base.TryFindResult(criteria, queryOptions, selector, out result);
    }

    public override void SaveFindResult<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector, TResult result)
    {
      if (GenerationalCachingEnabled)
        SetCache(FindCacheKey(criteria, queryOptions, selector), result);
    }

    public override void Add(TKey key, T result)
    {
      if (WriteThroughCachingEnabled)
        SetCache(GetWriteThroughCacheKey(key), result);

      if (GenerationalCachingEnabled)
        CheckPartitionUpdate(result);

    }

    public override void Update(TKey key, T result)
    {
      if (WriteThroughCachingEnabled)
        SetCache(GetWriteThroughCacheKey(key), result);

      if (GenerationalCachingEnabled)
        CheckPartitionUpdate(result);
    }

    public override void Delete(TKey key, T result)
    {
      if (WriteThroughCachingEnabled)
        ClearCache(GetWriteThroughCacheKey(key));

      if (GenerationalCachingEnabled)
        CheckPartitionUpdate(result);
    }

    public override void Save()
    {
      // data is being added/edited/deleted so we need to move to the next generation for this type
      IncrementGeneration();
    }

    // helpers

    /// <summary>
    /// Increments the partition generation if applicable based on the partitions value of the entity that was just added/updated/deleted
    /// </summary>
    /// <param name="result">The entity that was changed</param>
    private void CheckPartitionUpdate(T result)
    {
      // TODO: right now this is called multiple times in Batch mode even if 3 in a row are for the same partition
      //  this should batch up the calls to IncrementPartitionGeneration and only call once if there are 3 of the same partition values in the same batch
      if (TryPartitionValue(result, out TPartition partition))
      {
        IncrementPartitionGeneration(partition);
      }
    }

    public bool TryPartitionValue(T entity, out TPartition value)
    {
      value = default;

      if (Partition == null || entity == null)
        return false;


      if (Partition.Body is not MemberExpression partitionExpression)
        return false;

      var partitionName = partitionExpression.Member.Name;

      // use the partition name (which is a property) and reflection to get the value
      var type = typeof(T);
      var propInfo = type.GetTypeInfo().DeclaredProperties.FirstOrDefault(p => p.Name == partitionName && p.PropertyType == typeof(TPartition));

      if (propInfo == null)
        return false;

      value = (TPartition)propInfo.GetValue(entity, null);

      return true;
    }

    // TODO: make this private
    // shouldn't be public but wanted to test against it quickly right now
    public bool TryPartitionValue(ISpecification<T> criteria, out TPartition value)
    {
      value = default;

      if (Partition == null || criteria == null || criteria.Predicate == null)
        return false;


      if (Partition.Body is not MemberExpression partitionExpression)
        return false;

      var matches = new List<TPartition>();
      var partitionName = partitionExpression.Member.Name;

      RecurseExpressionTree(partitionName, criteria.Predicate.Body, ref matches);

      if (matches.Count == 1)
      {
        value = matches[0];
        return true;
      }

      return false;
    }

    private static void RecurseExpressionTree(string memberName, Expression expression, ref List<TPartition> matches)
    {
      if (expression is BinaryExpression binaryExpression)
      {

        var left = binaryExpression.Left;
        var right = binaryExpression.Right;

        // only equal works, if > or != then it can include more than just this partition so no need to check
        if (binaryExpression.NodeType == ExpressionType.Equal)
        {
          // check value is on the right
          if (left is MemberExpression expression1 && expression1.Member.Name == memberName)
          {
            if (right is ConstantExpression constantExpression)
            {
              matches.Add((TPartition)constantExpression.Value);
              return;
            }
            if (right is MemberExpression)
            {
              // this is when the right side is a variable instead of a constant like:
              //      var contactId = 1;
              //      var predicate = c => c.ContactId == contactId;
              // in this case we must compile the variable expression in order to evaluate it
              var accessorExpression = Expression.Lambda<Func<TPartition>>(right);
              matches.Add(accessorExpression.Compile()());
              return;
            }
          }

          // check value is on the left
          if (right is MemberExpression memberExpression && memberExpression.Member.Name == memberName)
          {
            if (left is ConstantExpression constantExpression)
            {
              matches.Add((TPartition)constantExpression.Value);
              return;
            }
            if (left is MemberExpression)
            {
              // this is when the right side is a variable instead of a constant like:
              //      var contactId = 1;
              //      var predicate = c => c.ContactId == contactId;
              // in this case we must compile the variable expression in order to evaluate it
              var accessorExpression = Expression.Lambda<Func<TPartition>>(left);
              matches.Add(accessorExpression.Compile()());
              return;
            }
          }
        }

        RecurseExpressionTree(memberName, left, ref matches);
        RecurseExpressionTree(memberName, right, ref matches);
      }
      else if (expression is MethodCallExpression methodCallExpression)
      {
        if (methodCallExpression.NodeType != ExpressionType.Call || methodCallExpression.Method.Name != "Equals" || methodCallExpression.Arguments.Count != 1 || methodCallExpression.Arguments[0].Type != typeof(TPartition))
          return;

        MemberExpression memberExpression;
        switch (methodCallExpression.Object.NodeType)
        {
          //  c.ContactTypeId.Equals(1) ... or ... c.ContactTypeId.Equals(contactTypeId) ... or ... contactTypeId.Equals(c.ContactTypeId)
          case ExpressionType.MemberAccess:
            switch (methodCallExpression.Arguments[0].NodeType)
            {
              case ExpressionType.Constant:
                matches.Add((TPartition)((ConstantExpression)methodCallExpression.Arguments[0]).Value);

                break;
              case ExpressionType.MemberAccess:
                // the argument can either be a variable or the Entity.Property, so let's figure out which
                memberExpression = methodCallExpression.Arguments[0] as MemberExpression;
                if (memberExpression == null)
                  return;

                if (memberExpression.Member.Name == memberName)
                {
                  // this means the argument is the Entity.Property
                  matches.Add(Expression.Lambda<Func<TPartition>>(methodCallExpression.Object).Compile()());
                }
                else if (((MemberExpression)methodCallExpression.Object).Member.Name == memberName)
                {
                  matches.Add(Expression.Lambda<Func<TPartition>>(methodCallExpression.Arguments[0]).Compile()());
                }

                break;
            }

            break;
          //  1.Equals(c.ContactTypeId)
          case ExpressionType.Constant:
            memberExpression = methodCallExpression.Arguments[0] as MemberExpression;
            if (memberExpression == null || memberExpression.Member.Name != memberName)
              return;

            var constantExpression = (ConstantExpression)methodCallExpression.Object;
            if (constantExpression == null || constantExpression.Type != typeof(TPartition))
              return;

            matches.Add((TPartition)constantExpression.Value);

            break;
        }
      }
    }

    protected override string GetAllCacheKey<TResult>(IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
    {
      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/{Md5Helper.CalculateMd5("All::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null"))}";
    }

    protected override string FindAllCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/FindAll/{Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null"))}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/FindAll/{Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null"))}";
    }

    protected override string FindCacheKey<TResult>(ISpecification<T> criteria, IQueryOptions<T> queryOptions, Expression<Func<T, TResult>> selector)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Find/{Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null"))}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Find/{Md5Helper.CalculateMd5(criteria + "::" + (queryOptions != null ? queryOptions.ToString() : "null") + "::" + (selector != null ? selector.ToString() : "null"))}";
    }

    protected override string CountCacheKey(ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Count/{Md5Helper.CalculateMd5(criteria == null ? "null" : criteria.ToString())}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Count/{Md5Helper.CalculateMd5(criteria == null ? "null" : criteria.ToString())}";
    }

    protected override string LongCountCacheKey(ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/LongCount/{Md5Helper.CalculateMd5(criteria == null ? "null" : criteria.ToString())}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/LongCount/{Md5Helper.CalculateMd5(criteria == null ? "null" : criteria.ToString())}";
    }

    protected override string GroupCountsCacheKey<TGroupKey>(Func<T, TGroupKey> keySelector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/GroupCounts/{Md5Helper.CalculateMd5((criteria == null ? "null" : criteria.ToString()) + "::" + keySelector + "::" + typeof(TGroupKey).FullName)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/GroupCounts/{Md5Helper.CalculateMd5((criteria == null ? "null" : criteria.ToString()) + "::" + keySelector + "::" + typeof(TGroupKey).FullName)}";
    }

    protected override string GroupCacheKey<TGroupKey, TResult>(Expression<Func<T, TGroupKey>> keySelector, Expression<Func<IGrouping<TGroupKey, T>, TResult>> resultSelector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Group/{Md5Helper.CalculateMd5((criteria == null ? "null" : criteria.ToString()) + "::" + keySelector + "::" + typeof(TGroupKey).FullName + "::" + resultSelector + "::" + typeof(TResult).FullName)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Group/{Md5Helper.CalculateMd5((criteria == null ? "null" : criteria.ToString()) + "::" + keySelector + "::" + typeof(TGroupKey).FullName + "::" + resultSelector + "::" + typeof(TResult).FullName)}";
    }

    protected override string SumCacheKey<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Sum/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Sum/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
    }

    protected override string AverageCacheKey<TSelector>(Expression<Func<T, TSelector>> selector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Average/{Md5Helper.CalculateMd5(typeof(TSelector).FullName + "::" + selector + "::" + criteria)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Average/{Md5Helper.CalculateMd5(typeof(TSelector).FullName + "::" + selector + "::" + criteria)}";
    }

    protected override string MinCacheKey<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Min/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Min/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
    }

    protected override string MaxCacheKey<TResult>(Expression<Func<T, TResult>> selector, ISpecification<T> criteria)
    {
      if (TryPartitionValue(criteria, out TPartition partition))
      {
        return
            $"{FullCachePrefix}/{TypeFullName}/p:{partition}/{GetPartitionGeneration(partition)}/Max/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
      }

      return
          $"{FullCachePrefix}/{TypeFullName}/{GetGeneration()}/Max/{Md5Helper.CalculateMd5(typeof(TResult).FullName + "::" + selector + "::" + criteria)}";
    }

    private int GetGeneration()
    {
      if (!GenerationalCachingEnabled) return 1; // no need to use the caching provider

      return !CachingProvider.Get(GetGenerationKey(), out int generation) ? 1 : generation;
    }

    private int IncrementGeneration()
    {
      return !GenerationalCachingEnabled ? 1 : CachingProvider.Increment(GetGenerationKey(), 1, 1, CacheItemPriority.NeverRemove);
    }

    private string GetGenerationKey()
    {
      return $"{FullCachePrefix}/{TypeFullName}/Generation";
    }

    private int GetPartitionGeneration(TPartition partition)
    {
      if (!GenerationalCachingEnabled) return 1; // no need to use the caching provider

      return !CachingProvider.Get(GetPartitionGenerationKey(partition), out int generation) ? 1 : generation;
    }

    private int IncrementPartitionGeneration(TPartition partition)
    {
      return !GenerationalCachingEnabled ? 1 : CachingProvider.Increment(GetPartitionGenerationKey(partition), 1, 1, CacheItemPriority.NeverRemove);
    }

    protected string GetPartitionGenerationKey(TPartition partition)
    {
      return $"{FullCachePrefix}/{TypeFullName}/p:{partition}/Generation";
    }
  }
}
