using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public class RepositoryQueryMultipleContext<T, TKey> : RepositoryQueryMultipleContext<T, TKey, T>
      where T : class
  {
    public RepositoryQueryMultipleContext(
        IRepository<T, TKey> repository,
        ISpecification<T> specification,
        IQueryOptions<T> queryOptions)
        : base(repository, specification, queryOptions, null)
    {
    }
  }
}