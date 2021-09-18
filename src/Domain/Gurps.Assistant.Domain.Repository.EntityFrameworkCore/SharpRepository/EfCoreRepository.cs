using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository
{

  public class EfCoreRepository<T, TKey, TKey2, TKey3> : EfCoreCompoundKeyRepositoryBase<T, TKey, TKey2, TKey3>
        where T : class
  {
    public EfCoreRepository(ICoreDbContext contextFactory, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null)
        : base(contextFactory, cachingStrategy)
    {
    }
  }

  public class EfCoreRepository<T, TKey, TKey2> : EfCoreCompoundKeyRepositoryBase<T, TKey, TKey2>
      where T : class
  {
    public EfCoreRepository(ICoreDbContext contextFactory, ICompoundKeyCachingStrategy<T, TKey, TKey2> cachingStrategy = null)
        : base(contextFactory, cachingStrategy)
    {
    }
  }

  public class EfCoreCompoundKeyRepository<T> : EfCoreCompoundKeyRepositoryBase<T>
      where T : class
  {
    public EfCoreCompoundKeyRepository(ICoreDbContext contextFactory, ICompoundKeyCachingStrategy<T> cachingStrategy = null)
        : base(contextFactory, cachingStrategy)
    {
    }
  }

  /// <summary>
  /// Entity Framework repository layer
  /// </summary>
  /// <typeparam name="T">The Entity type</typeparam>
  /// <typeparam name="TKey">The type of the primary key.</typeparam>
  /// <typeparam name = "TContext">Data context </typeparam>
  public class EfCoreRepository<T, TKey> : EfCoreRepositoryBase<T, TKey>
      where T : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EfRepository&lt;T, TKey&gt;"/> class.
    /// </summary>
    /// <param name="dbContext">The Entity Framework DbContext.</param>
    /// <param name = "dataContextFactory">data Context Factory </param>
    /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy{T}" /></param>
    public EfCoreRepository(ICoreDbContext contextFactory, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(contextFactory, cachingStrategy)
    {
    }
  }

  /// <summary>
  /// Entity Framework repository layer
  /// </summary>
  /// <typeparam name="T">The Entity type</typeparam>
  public class EfCoreRepository<T> : EfCoreRepositoryBase<T, int>, IRepository<T>
      where T : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EfRepository&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="dbContext">The Entity Framework DbContext.</param>
    /// <param name="cachingStrategy">The caching strategy to use.  Defaults to <see cref="NoCachingStrategy&lt;T, TKey&gt;" /></param>
    public EfCoreRepository(ICoreDbContext contextFactory, ICachingStrategy<T, int> cachingStrategy = null)
        : base(contextFactory, cachingStrategy)
    {
    }
  }
}
