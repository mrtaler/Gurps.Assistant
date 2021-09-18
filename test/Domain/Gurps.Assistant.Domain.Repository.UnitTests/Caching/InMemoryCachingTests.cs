using System;
using System.Collections.Concurrent;
using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Caching
{

  public class InMemoryCachingTests : TestBase, IDisposable
  {
    private ICachingProvider cacheProvider;

    public InMemoryCachingTests()
    {
      // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
      cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions { }));
    }

    public void Dispose()
    {
      cacheProvider.Dispose();
    }

    [Fact]
    public void ExecuteGetAll_With_Selector_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var items = repos.GetAll(x => x.Name);
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(2);

      items = repos.GetAll(x => x.Name);
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(2);
    }

    [Fact]
    public void ExecuteGetAll_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var items = repos.GetAll();
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(2);

      items = repos.GetAll();
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(2);
    }

    [Fact]
    public void ExecuteFindAll_With_Selector_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var items = repos.FindAll(x => x.ContactId < 3, x => x.Name);
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(2);

      items = repos.FindAll(x => x.ContactId < 3, x => x.Name);
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(2);
    }

    [Fact]
    public void ExecuteFindAll_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var items = repos.FindAll(x => x.ContactId < 3);
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(2);

      items = repos.FindAll(x => x.ContactId < 3);
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(2);
    }

    [Fact]
    public void ExecuteFind_With_Selector_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var item = repos.Find(x => x.ContactId == 1, x => x.Name);
      repos.CacheUsed.Should().BeFalse();
      item.Should().NotBeNull();

      item = repos.Find(x => x.ContactId == 1, x => x.Name);
      repos.CacheUsed.Should().BeTrue();
      item.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteFind_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var item = repos.Find(x => x.ContactId == 1);
      repos.CacheUsed.Should().BeFalse();
      item.Should().NotBeNull();

      item = repos.Find(x => x.ContactId == 1);
      repos.CacheUsed.Should().BeTrue();
      item.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteGet_With_Selector_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var item = repos.Get(1, x => x.Name);
      repos.CacheUsed.Should().BeTrue();
      item.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteGet_Should_Use_Cache_After_First_Call()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { Name = "Test1" });
      repos.Add(new Contact { Name = "Test2" });

      var item = repos.Get(1);
      repos.CacheUsed.Should().BeTrue();
      item.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteFindAll_With_Paging_Should_Save_TotalItems_In_Cache()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { ContactId = 1, Name = "Test1" });
      repos.Add(new Contact { ContactId = 2, Name = "Test2" });
      repos.Add(new Contact { ContactId = 3, Name = "Test3" });
      repos.Add(new Contact { ContactId = 4, Name = "Test4" });

      var pagingOptions = new PagingOptions<Contact>(1, 1, "Name");

      var items = repos.FindAll(x => x.ContactId >= 2, x => x.Name, pagingOptions);
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(1);
      pagingOptions.TotalItems.Should().Be(3);

      // reset paging options so the TotalItems is default
      pagingOptions = new PagingOptions<Contact>(1, 1, "Name");

      items = repos.FindAll(x => x.ContactId >= 2, x => x.Name, pagingOptions);
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(1);
      pagingOptions.TotalItems.Should().Be(3);
    }

    [Fact]
    public void ExecuteGetAll_With_Paging_Should_Save_TotalItems_In_Cache()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact>(cacheProvider));

      repos.Add(new Contact { ContactId = 1, Name = "Test1" });
      repos.Add(new Contact { ContactId = 2, Name = "Test2" });
      repos.Add(new Contact { ContactId = 3, Name = "Test3" });
      repos.Add(new Contact { ContactId = 4, Name = "Test4" });

      var pagingOptions = new PagingOptions<Contact>(1, 1, "Name");

      var items = repos.GetAll(x => x.Name, pagingOptions);
      repos.CacheUsed.Should().BeFalse();
      items.Count().Should().Be(1);
      pagingOptions.TotalItems.Should().Be(4);

      // reset paging options so the TotalItems is default
      pagingOptions = new PagingOptions<Contact>(1, 1, "Name");

      items = repos.GetAll(x => x.Name, pagingOptions);
      repos.CacheUsed.Should().BeTrue();
      items.Count().Should().Be(1);
      pagingOptions.TotalItems.Should().Be(4);
    }
  }
}
