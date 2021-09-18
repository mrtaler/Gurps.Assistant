using Gurps.Assistant.Domain.Repository.Caching;
using Raven.Client.Documents.Session;

namespace Gurps.Assistant.Domain.Repository.RavenDb.SharpRepository
{
  /// <summary>
  /// RavenDb repository layer
  /// </summary>
  /// <typeparam name="T">The type of object the repository acts on.</typeparam>
  /// <typeparam name="TKey">The type of the primary key.</typeparam>
  public class RavenDbRepository<TEntity, TKey> : RavenDbRepositoryBase<TEntity, TKey>
        where TEntity : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RavenDbRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy{T}" />.</param>
    public RavenDbRepository(IDocumentSession session,
        ICachingStrategy<TEntity, TKey> cachingStrategy = null)
        : base(session, cachingStrategy)
    {
    }
  }

  /// <summary>
  /// RavenDb repository layer
  /// </summary>
  /// <typeparam name="T">The type of object the repository acts on.</typeparam>
  public class RavenDbRepository<TEntity> : RavenDbRepositoryBase<TEntity, string>
      where TEntity : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RavenDbRepository{TEntity}"/> class.
    /// </summary>
    /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
    public RavenDbRepository(IDocumentSession session, ICachingStrategy<TEntity, string> cachingStrategy = null)
        : base(session, cachingStrategy)
    {
    }
  }
}