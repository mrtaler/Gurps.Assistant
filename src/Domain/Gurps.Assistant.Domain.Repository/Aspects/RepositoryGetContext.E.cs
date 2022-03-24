using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.Aspects
{
  public class RepositoryGetContext<T, TKey> : RepositoryGetContext<T, TKey, T>
      where T : class
  {
    public RepositoryGetContext(IRepository<T, TKey> repository, TKey id) : base(repository, id)
    {
    }
  }
}