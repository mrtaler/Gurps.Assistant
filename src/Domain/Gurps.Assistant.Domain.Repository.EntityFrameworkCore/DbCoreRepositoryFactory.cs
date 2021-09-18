using Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore
{
  public class DbCoreRepositoryFactory<TContext> : RepositoryFactoryBase<DbCoreContextFactory<TContext>, DbContext>
        where TContext : DbContext, ICoreDbContext
  {
    public DbCoreRepositoryFactory(DbCoreContextFactory<TContext> dataCoreContextFactory) : base(dataCoreContextFactory)
    {
    }

    /// <inheritdoc />
    public override IRepository<T> GetInstance<T>()
    {
      return new EfCoreRepository<T>(DataContextFactory.GetContext());
    }

    /// <inheritdoc />
    public override IRepository<T, TKey> GetInstance<T, TKey>()
    {
      return new EfCoreRepository<T, TKey>(DataContextFactory.GetContext());
    }

    /// <inheritdoc />
    public override ICompoundKeyRepository<T, TKey, TKey2> GetInstance<T, TKey, TKey2>()
    {
      return new EfCoreRepository<T, TKey, TKey2>(DataContextFactory.GetContext());
    }

    /// <inheritdoc />
    public override ICompoundKeyRepository<T, TKey, TKey2, TKey3> GetInstance<T, TKey, TKey2, TKey3>()
    {
      return new EfCoreRepository<T, TKey, TKey2, TKey3>(DataContextFactory.GetContext());
    }
  }
}