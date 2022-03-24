using System;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.Interfaces
{
  public interface IUnitOfWork : IDisposable
  {
    int? SaveChanges();

    IRepositoryFactory GetRepositoryFactory();

    IRepository<T> GetRepository<T>() where T : class, new();
    IRepository<T, TKey> GetRepository<T, TKey>() where T : class, new();
    ICompoundKeyRepository<T, TKey, TKey2> GetRepository<T, TKey, TKey2>() where T : class, new();
    ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetRepository<T, TKey, TKey2, TKey3>() where T : class, new();
  }

  public interface IUnitOfWork<TContext, out TDataContextFactory> : IUnitOfWork
      where TContext : class
      where TDataContextFactory : IDataContextFactory<TContext>
  {
    TDataContextFactory DataContextFactory { get; }
  }
}