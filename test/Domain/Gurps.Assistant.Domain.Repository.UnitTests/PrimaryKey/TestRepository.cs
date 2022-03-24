using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Gurps.Assistant.Domain.Repository.Attributes;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.InMemory.RolePlayedGamesHelper.Repository.InMemoryRepository.SharpRepository;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;

namespace Gurps.Assistant.Domain.Repository.UnitTests.PrimaryKey
{
  internal class TestRepository<T, TKey> : InMemRepository<T, TKey> where T : class, new()
  {
    public PropertyInfo TestGetPrimaryKeyPropertyInfo()
    {
      return GetPrimaryKeyPropertyInfo();
    }

    public void SuppressAudit()
    {
      DisableAspect(typeof(AuditAttributeMock));
    }

    public void RestoreAudit()
    {
      EnableAspect(typeof(AuditAttributeMock));
    }

    public IEnumerable<RepositoryActionBaseAttribute> GetAspects()
    {
      return Aspects;
    }

    /// <inheritdoc />
    public TestRepository(ConcurrentDictionary<TKey, T> items, ICachingStrategy<T, TKey> cachingStrategy = null)
        : base(items, cachingStrategy)
    {
    }
  }
}