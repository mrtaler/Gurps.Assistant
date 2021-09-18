using System.Reflection;

namespace Gurps.Assistant.Domain.Repository.UnitTests.PrimaryKey
{
  public class TestEfCoreRepository<T, TKey> : EfCoreRepository<T, TKey> where T : class, new()
  {
    public TestEfCoreRepository(ICoreDbContext dbContext, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(dbContext, cachingStrategy)
    {
    }

    public PropertyInfo TestGetPrimaryKeyPropertyInfo()
    {
      return GetPrimaryKeyPropertyInfo();
    }
  }
}