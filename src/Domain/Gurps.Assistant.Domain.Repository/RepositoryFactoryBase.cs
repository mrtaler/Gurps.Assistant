using Gurps.Assistant.Domain.Repository.Interfaces;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository
{
  public abstract class RepositoryFactoryBase<TDataContextFactory, TContext> :
        IRepositoryFactory<TDataContextFactory, TContext>
        where TDataContextFactory : IDataContextFactory<TContext>
        where TContext : class
  {
    public TDataContextFactory DataContextFactory { get; }

    protected RepositoryFactoryBase(TDataContextFactory dataContextFactory)
    {
      DataContextFactory = dataContextFactory;
    }

    /// <inheritdoc />
    public abstract IRepository<T> GetInstance<T>() where T : class, new();

    /// <inheritdoc />
    public abstract IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();

    /// <inheritdoc />
    public abstract ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();

    /// <inheritdoc />
    public abstract ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
  }
}
