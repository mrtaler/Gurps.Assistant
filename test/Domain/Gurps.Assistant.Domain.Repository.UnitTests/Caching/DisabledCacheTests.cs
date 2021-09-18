using System.Collections.Concurrent;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Caching
{

  public class DisabledCacheTests
  {
    [Fact]
    public void Using_DisableCaching_Should_Disable_Cache_Inside_Using_Block()
    {
      var cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
      var repos = new InMemRepository<Contact>(
          new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.CachingEnabled.Should().BeTrue();

      using (repos.DisableCaching())
      {
        repos.CachingEnabled.Should().BeFalse();
      }

      repos.CachingEnabled.Should().BeTrue();
    }
  }
}
