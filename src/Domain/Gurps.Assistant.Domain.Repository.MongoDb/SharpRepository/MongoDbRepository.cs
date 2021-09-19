using Gurps.Assistant.Domain.Repository.Caching;
using MongoDB.Driver;

namespace Gurps.Assistant.Domain.Repository.MongoDb.SharpRepository
{
  /// <summary>
  /// MongoDb repository layer
  /// </summary>
  /// <typeparam name="T">The type of object the repository acts on.</typeparam>
  /// <typeparam name="TKey">The type of the primary key.</typeparam>
  public class MongoDbRepository<T, TKey> : MongoDbRepositoryBase<T, TKey>
        where T : class, new()
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T, TKey&gt;"/> class.
    /// </summary>
    /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy{T}" />.</param>
    public MongoDbRepository(IMongoCollection<T> collection, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(collection, cachingStrategy)
    {
    }
  }

  /// <summary>
  /// MongoDb repository layer
  /// </summary>
  /// <typeparam name="T">The type of object the repository acts on.</typeparam>
  public class MongoDbRepository<T> : MongoDbRepositoryBase<T, string>
      where T : class, new()
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDbRepository&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="cachingStrategy">The caching strategy.  Defaults to <see cref="NoCachingStrategy&lt;T&gt;" />.</param>
    public MongoDbRepository(IMongoCollection<T> collection, ICachingStrategy<T, string> cachingStrategy = null)
        : base(collection, cachingStrategy)
    {
    }
  }
}