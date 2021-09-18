using System;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.PrimaryKey
{
  public class EfCorePrimaryKeyTests : IDisposable
  {
    protected TestObjectContextCore context;

    public EfCorePrimaryKeyTests()
    {
      /* var connection = new SqliteConnection("DataSource=:memory:");
       connection.Open();*/

      var options = new DbContextOptionsBuilder<TestObjectContextCore>()
          .UseInMemoryDatabase("memory")
          .Options;



      // Run the test against one instance of the contextFactory
      context = new TestObjectContextCore(options);


    }

    public void Dispose()
    {
      context.Dispose();
      context = null;
    }

    [Fact]
    public void Should_Return_ContactId_Property()
    {
      var repos = new TestEfCoreRepository<Contact, int>(context);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.PropertyType.Should().Be(typeof(int));
      propInfo.Name.Should().Be("ContactId");
    }

    /*  [Fact]
      public void Should_Return_Some_Another_Last_Id_Property()
      {
          var repos = new TestTripleKeyEfCoreRepository<TripleCompoundKeyItemInts, int, int, int>(contextFactory);
          var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

          propInfo[0].PropertyType.Should().Be(typeof(int));
          propInfo[0].Name.Should().Be("SomeId");
          propInfo[1].PropertyType.Should().Be(typeof(int));
          propInfo[1].Name.Should().Be("AnotherId");
          propInfo[2].PropertyType.Should().Be(typeof(int));
          propInfo[2].Name.Should().Be("LastId");
      }*/
  }

  /*  internal class TestTripleKeyEfCoreRepository<T, TKey, TKey2, TKey3> : EfCoreRepository<T, TKey, TKey2, TKey3> where T : class, new()
  {
      public TestTripleKeyEfCoreRepository(DbContext dbContext, ICompoundKeyCachingStrategy<T, TKey, TKey2, TKey3> cachingStrategy = null) : base(dbContext, cachingStrategy)
      {
      }

      public PropertyInfo[] TestGetPrimaryKeyPropertyInfo()
      {
          return GetPrimaryKeyPropertyInfo();
      }
  }*/
}
