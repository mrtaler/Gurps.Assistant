using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public abstract class RepositoryQueryContext<T, TKey> : RepositoryQueryContext<T, TKey, T> where T : class
  {
    protected RepositoryQueryContext(IRepository<T, TKey> repository, ISpecification<T> specification, IQueryOptions<T> queryOptions)
        : base(repository, specification, queryOptions)
    {
    }
  }
}