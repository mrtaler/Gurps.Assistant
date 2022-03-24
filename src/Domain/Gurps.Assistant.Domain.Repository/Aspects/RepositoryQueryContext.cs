using System;
using System.Linq.Expressions;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public abstract class RepositoryQueryContext<T, TKey, TResult> : RepositoryActionContext<T, TKey>
        where T : class
  {
    protected RepositoryQueryContext(
        IRepository<T, TKey> repository,
        ISpecification<T> specification,
        IQueryOptions<T> queryOptions,
        Expression<Func<T, TResult>> selector = null)
        : base(repository)
    {
      Specification = specification;
      QueryOptions = queryOptions;
      Selector = selector;
    }

    public ISpecification<T> Specification { get; set; }
    public IQueryOptions<T> QueryOptions { get; set; }
    public virtual int NumberOfResults { get; internal set; }
    public Expression<Func<T, TResult>> Selector { get; set; }
  }
}