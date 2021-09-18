using System;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.FetchStrategies;

namespace Gurps.Assistant.Domain.Repository.Helpers
{
  public static class RepositoryHelper
  {
    public static IFetchStrategy<T> BuildFetchStrategy<T>(params string[] includePaths)
    {
      var fetchStrategy = new GenericFetchStrategy<T>();
      foreach (var path in includePaths)
      {
        fetchStrategy.Include(path);
      }
      return fetchStrategy;
    }

    public static IFetchStrategy<T> BuildFetchStrategy<T>(params Expression<Func<T, object>>[] includePaths)
    {
      var fetchStrategy = new GenericFetchStrategy<T>();
      foreach (var path in includePaths)
      {
        fetchStrategy.Include(path);
      }
      return fetchStrategy;
    }
  }
}
