using System;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public class RepositoryQuerySingleContext<T, TKey, TResult> : RepositoryQueryContext<T, TKey, TResult>
        where T : class
  {
    public RepositoryQuerySingleContext(
        IRepository<T, TKey> repository,
        ISpecification<T> specification,
        IQueryOptions<T> queryOptions,
        Expression<Func<T, TResult>> selector = null)
        : base(repository, specification, queryOptions, selector)
    {
    }

    public TResult Result { get; set; }

    public bool HasResult
    {
      get { return NumberOfResults != 0; }
    }
    public override int NumberOfResults
    {
      get
      {
        return Result == null || Result.Equals(default(TResult)) ? 0 : 1;
      }
    }
  }
}