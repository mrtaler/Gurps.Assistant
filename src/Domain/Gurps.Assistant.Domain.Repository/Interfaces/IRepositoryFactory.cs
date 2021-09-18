using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.Interfaces
{
  public interface IRepositoryFactory
  {
    IRepository<T> GetInstance<T>() where T : class, new();
    IRepository<T, TKey> GetInstance<T, TKey>() where T : class, new();
    ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>() where T : class, new();
    ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>() where T : class, new();
  }

  public interface IRepositoryFactory<out TDataContextFactory, TContext> : IRepositoryFactory
      where TDataContextFactory : IDataContextFactory<TContext>
      where TContext : class
  {
    TDataContextFactory DataContextFactory { get; }
  }
}
