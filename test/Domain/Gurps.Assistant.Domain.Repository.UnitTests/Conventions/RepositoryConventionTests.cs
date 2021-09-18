using System.Collections.Concurrent;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.InMemory.RolePlayedGamesHelper.Repository.InMemoryRepository.SharpRepository;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Conventions
{
  public class RepositoryConventionTests
  {
    private ICachingProvider cacheProvider;

    public RepositoryConventionTests()
    {
      cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public void Default_PrimaryKeySuffix_Is_Id()
    {
      DefaultRepositoryConventions.PrimaryKeySuffix.Should().Be("Id");
    }

    [Fact]
    public void RepositoryConventions_Uses_Default_PrimaryKeySuffix()
    {
      DefaultRepositoryConventions.PrimaryKeySuffix = "Key";
      DefaultRepositoryConventions.GetPrimaryKeyName(typeof(Contact)).Should().BeNull();

      // change back to default for the rest of the tests
      DefaultRepositoryConventions.PrimaryKeySuffix = "Id";
    }

    [Fact]
    public void Default_PrimaryKeyName()
    {
      DefaultRepositoryConventions.GetPrimaryKeyName(typeof(Contact)).Should().Be("ContactId");
    }

    [Fact]
    public void Change_PrimaryKeyName()
    {
      var orig = DefaultRepositoryConventions.GetPrimaryKeyName;

      DefaultRepositoryConventions.GetPrimaryKeyName = type => "PK_" + type.Name + "_Id";

      DefaultRepositoryConventions.GetPrimaryKeyName(typeof(TestConventionObject)).Should().Be("PK_TestConventionObject_Id");

      DefaultRepositoryConventions.GetPrimaryKeyName = orig;
    }

    [Fact]
    public void Default_CachePrefix()
    {
      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact, int>(cacheProvider));
      repos.CachingStrategy.FullCachePrefix.Should().StartWith(DefaultRepositoryConventions.CachePrefix);
    }

    [Fact]
    public void Change_CachePrefix()
    {
      const string newPrefix = "TestPrefix123";
      DefaultRepositoryConventions.CachePrefix = newPrefix;

      var repos = new InMemRepository<Contact>(
              new ConcurrentDictionary<int, Contact>(), new StandardCachingStrategy<Contact, int>(cacheProvider));
      repos.CachingStrategy.FullCachePrefix.Should().StartWith(newPrefix);
    }

    /*   [Fact]
     public void RavenDb_Throws_NotSupportedException_When_GenerateKeyOnAdd_Is_Set_False()
       {
           var ravenDbRepo = new RavenDbRepository<Contact>();
           Exception actualException = null;

           try
           {
               ravenDbRepo.GenerateKeyOnAdd = false;
           }
           catch (Exception ex)
           {
               actualException = ex;
           }

           actualException.Should().NotBeNull();
           actualException.Should().BeOfType<NotSupportedException>();
       }*/

    internal class TestConventionObject
    {
      public int PK_TestConventionObject_Id { get; set; }
      public string Name { get; set; }
    }
  }
}
