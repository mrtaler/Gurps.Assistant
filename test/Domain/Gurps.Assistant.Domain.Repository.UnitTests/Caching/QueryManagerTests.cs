using System;
using System.Collections.Generic;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.Specifications;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Microsoft.Extensions.Caching.Memory;
using RolePlayedGamesHelper.Repository.SharpRepository.Queries;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Caching
{

  public class QueryManagerTests : TestBase, IDisposable
  {
    protected QueryManager<Contact, int> QueryManager;
    protected MemoryCache cache;

    public QueryManagerTests()
    {
      // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
      cache = new MemoryCache(new MemoryCacheOptions());
      var provider = new InMemoryCachingProvider(cache);
      QueryManager = new QueryManager<Contact, int>(new StandardCachingStrategy<Contact, int>(provider)
      {
        CachePrefix = "#RepoStandardCache"
      });
    }

    public void Dispose()
    {
      //Repository = null;
      cache.Dispose();
    }

    [Fact]
    public void ExecuteGet_Should_Not_Use_Cache()
    {
      QueryManager.ExecuteGet(FakeGet, 1);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteGet_Should_Use_Cache_After_First_Call()
    {
      // first time no cache yet
      QueryManager.ExecuteGet(FakeGet, 1);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time the cache has been populated from the last call
      QueryManager.ExecuteGet(FakeGet, 1);
      QueryManager.CacheUsed.Should().BeTrue();
    }

    [Fact]
    public void ExecuteGet_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
    {
      // first time no cache yet
      QueryManager.ExecuteGet(FakeGet, 1);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time the cache has been populated from the last call
      QueryManager.CacheEnabled = false;
      QueryManager.ExecuteGet(FakeGet, 1);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteGetAll_Should_Not_Use_Cache()
    {
      QueryManager.ExecuteGetAll(FakeGetAll, null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteGetAll_Should_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteGetAll(FakeGetAll, null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.ExecuteGetAll(FakeGetAll, null, null);
      QueryManager.CacheUsed.Should().BeTrue();
    }

    [Fact]
    public void ExecuteGetAll_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteGetAll(FakeGetAll, null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.CacheEnabled = false;
      QueryManager.ExecuteGetAll(FakeGetAll, null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteFindAll_Should_Not_Use_Cache()
    {
      QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteFindAll_Should_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeTrue();
    }

    [Fact]
    public void ExecuteFindAll_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.CacheEnabled = false;
      QueryManager.ExecuteFindAll(FakeGetAll, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteFind_Should_Not_Use_Cache()
    {
      QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }

    [Fact]
    public void ExecuteFind_Should_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeTrue();
    }

    [Fact]
    public void ExecuteFind_Cache_Disabled_Should_Not_Use_Cache_After_First_Call()
    {
      // first time should not find anything
      QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();

      // second time it should be from cache
      QueryManager.CacheEnabled = false;
      QueryManager.ExecuteFind(FakeGet, new Specification<Contact>(c => c.ContactId < 10), null, null);
      QueryManager.CacheUsed.Should().BeFalse();
    }


    #region fake calls

    public Contact FakeGet()
    {
      return new Contact();
    }

    public IEnumerable<Contact> FakeGetAll()
    {
      return new List<Contact>();
    }

    #endregion
  }
}
