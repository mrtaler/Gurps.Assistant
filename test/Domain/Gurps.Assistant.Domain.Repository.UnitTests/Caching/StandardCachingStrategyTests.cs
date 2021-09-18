using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.Queries;
using Gurps.Assistant.Domain.Repository.Specifications;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Caching
{

  public class StandardCachingStrategyTests : TestBase, IDisposable
  {
    protected ICachingStrategy<Contact, int> CachingStrategy;


    public StandardCachingStrategyTests()
    {
      // need to clear out the InMemory cache before each test is run so that each is independent and won't effect the next one
      var provider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
      CachingStrategy = new StandardCachingStrategy<Contact, int>(provider) { CachePrefix = "#RepoStandardCache" };
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    [Fact]
    public void TryGetResult_First_Call_Should_Return_False()
    {
      CachingStrategy.TryGetResult(1, out Contact result).Should().Be(false);
      result.Should().Be(null);
    }

    [Fact]
    public void SaveGetResult_Should_Set_Cache()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };

      CachingStrategy.SaveGetResult(1, contact);
      CachingStrategy.TryGetResult(1, out Contact result).Should().Be(true);

      result.ContactId.Should().Be(contact.ContactId);
      result.Name.Should().Be(contact.Name);
    }

    [Fact]
    public void SaveGetResult_With_WriteThrough_Disabled_Should_Not_Set_Cache()
    {
      ((StandardCachingStrategy<Contact, int>)CachingStrategy).WriteThroughCachingEnabled = false;

      var contact = new Contact() { ContactId = 1, Name = "Test User" };

      CachingStrategy.SaveGetResult(1, contact);
      CachingStrategy.TryGetResult(1, out _).Should().Be(false);
    }

    [Fact]
    public void TryGetAllResult_First_Call_Should_Return_False()
    {
      CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).Should().Be(false);
      result.Should().BeNull();
    }

    [Fact]
    public void SaveGetAllResult_Should_Set_Cache()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };

      CachingStrategy.SaveGetAllResult(null, null, new[] { contact });
      CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> result).Should().Be(true);

      result.Count().Should().Be(1);
    }

    [Fact]
    public void SaveGetAllResult_With_Generational_Disabled_Should_Not_Set_Cache()
    {
      ((StandardCachingStrategy<Contact, int>)CachingStrategy).GenerationalCachingEnabled = false;

      var contact = new Contact() { ContactId = 1, Name = "Test User" };

      CachingStrategy.SaveGetAllResult(null, null, new[] { contact });
      CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> _).Should().Be(false);
    }

    [Fact]
    public void TryGetAllResult_With_Different_QueryOptions_Should_Return_False()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };

      CachingStrategy.SaveGetAllResult(new SortingOptions<Contact>("Name"), null, new[] { contact });
      CachingStrategy.TryGetAllResult(null, null, out IEnumerable<Contact> _).Should().Be(false);
    }

    [Fact]
    public void TryGetAllResult_With_Same_QueryOptions_Should_Return_True()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var sorting = new SortingOptions<Contact>("Name");

      CachingStrategy.SaveGetAllResult(sorting, null, new[] { contact });
      CachingStrategy.TryGetAllResult(sorting, null, out IEnumerable<Contact> result).Should().Be(true);

      result.Count().Should().Be(1);
    }

    [Fact]
    public void TryFindAllResult_First_Call_Should_Return_False()
    {
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = null;

      CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).Should().Be(false);
      result.Should().BeNull();
    }

    [Fact]
    public void SaveFindAllResult_Should_Set_Cache()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = null;

      CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
      CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).Should().Be(true);
      result.Count().Should().Be(1);

      queryOptions = new SortingOptions<Contact>("Name");
      CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
      CachingStrategy.TryFindAllResult(specification, queryOptions, null, out result).Should().Be(true);
      result.Count().Should().Be(1);
    }

    [Fact]
    public void TryFindAllResult_With_Different_QueryOptions_Should_Return_False()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

      CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
      CachingStrategy.TryFindAllResult(specification, null, null, out IEnumerable<Contact> result).Should().Be(false);


      CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
      CachingStrategy.TryFindAllResult(specification, new SortingOptions<Contact>("Name", true), null, out result).Should().Be(false);
    }

    [Fact]
    public void TryFindAllResult_With_Same_QueryOptions_Should_Return_True()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

      CachingStrategy.SaveFindAllResult(specification, queryOptions, null, new[] { contact });
      CachingStrategy.TryFindAllResult(specification, queryOptions, null, out IEnumerable<Contact> result).Should().Be(true);

      result.Count().Should().Be(1);
    }

    [Fact]
    public void TryFindResult_First_Call_Should_Return_False()
    {
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = null;

      CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).Should().Be(false);
      result.Should().Be(null);
    }

    [Fact]
    public void SaveFindResult_Should_Set_Cache()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = null;

      CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
      CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).Should().Be(true);

      queryOptions = new SortingOptions<Contact>("Name");
      CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
      CachingStrategy.TryFindResult(specification, queryOptions, null, out result).Should().Be(true);
    }

    [Fact]
    public void TryFindResult_With_Different_QueryOptions_Should_Return_False()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

      CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
      CachingStrategy.TryFindResult(specification, null, null, out Contact result).Should().Be(false);


      CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
      CachingStrategy.TryFindResult(specification, new SortingOptions<Contact>("Name", true), null, out result).Should().Be(false);
    }

    [Fact]
    public void TryFindResult_With_Same_QueryOptions_Should_Return_True()
    {
      var contact = new Contact() { ContactId = 1, Name = "Test User" };
      var specification = new Specification<Contact>(x => x.ContactId == 1);
      IQueryOptions<Contact> queryOptions = new SortingOptions<Contact>("Name");

      CachingStrategy.SaveFindResult(specification, queryOptions, null, contact);
      CachingStrategy.TryFindResult(specification, queryOptions, null, out Contact result).Should().Be(true);
    }
  }
}
