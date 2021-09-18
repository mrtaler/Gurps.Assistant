using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.Specifications;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Spikes
{
  public class ExpressionParsingSpike : TestBase
  {
    private ICachingProvider cacheProvider;

    public ExpressionParsingSpike()
    {
      cacheProvider = new InMemoryCachingProvider(new MemoryCache(new MemoryCacheOptions()));
    }

    [Fact]
    public void Get_Entity_Partition_Value()
    {
      var contact1 = new Contact { ContactId = 1, ContactTypeId = 1 };
      var contact2 = new Contact { ContactId = 1, ContactTypeId = 2 };
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact>(cacheProvider, c => c.ContactTypeId);

      cachingStrategy.TryPartitionValue(contact1, out int contactId);
      contactId.Should().Be(1);

      cachingStrategy.TryPartitionValue(contact2, out contactId);
      contactId.Should().Be(2);
    }

    [Fact]
    public void Single_Part_Predicate_Constant_On_Right_Should_Not_Match()
    {
      var spec = new Specification<Contact>(contact => contact.Name == "test");
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(false);
      value.Should().Be(0);
    }

    [Fact]
    public void Single_Part_Predicate_GreaterThan_Should_Not_Match()
    {
      var spec = new Specification<Contact>(contact => contact.ContactTypeId > 1);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(false);
      value.Should().Be(0);
    }

    [Fact]
    public void Single_Part_Predicate_NotEqual_Should_Not_Match()
    {
      var spec = new Specification<Contact>(contact => contact.ContactTypeId != 1);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(false);
      value.Should().Be(0);
    }

    [Fact]
    public void Single_Part_Predicate_Constant_On_Right_Should_Match()
    {
      var spec = new Specification<Contact>(contact => contact.ContactTypeId == 1);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Using_Equals_Method_On_Right_Should_Match()
    {
      var spec = new Specification<Contact>(contact => contact.ContactTypeId.Equals(1));
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Using_Equals_Method_On_Right_With_Variable_Should_Match()
    {
      var contactTypeId = 1;
      var spec = new Specification<Contact>(contact => contact.ContactTypeId.Equals(contactTypeId));
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Using_Equals_Method_On_Left_Should_Match()
    {
      var spec = new Specification<Contact>(contact => 1.Equals(contact.ContactTypeId));
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Using_Equals_Method_On_Left_With_Variable_Should_Match()
    {
      var contactTypeId = 1;
      var spec = new Specification<Contact>(contact => contactTypeId.Equals(contact.ContactTypeId));
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Constant_On_Left_Should_Match()
    {
      var spec = new Specification<Contact>(contact => 1 == contact.ContactTypeId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Variable_On_Right_Should_Match()
    {
      var contactId = 1;

      var spec = new Specification<Contact>(contact => contact.ContactTypeId == contactId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Single_Part_Predicate_Variable_On_Left_Should_Match()
    {
      var contactId = 1;

      var spec = new Specification<Contact>(contact => contactId == contact.ContactTypeId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Two_Part_Predicate_Constant_On_Right_Should_Match()
    {
      var spec = new Specification<Contact>(contact => contact.Name == "test" && contact.ContactTypeId == 1);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Two_Part_Predicate_Constant_On_Left_Should_Match()
    {
      var spec = new Specification<Contact>(contact => contact.Name == "test" && 1 == contact.ContactTypeId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Two_Part_Predicate_Variable_On_Right_Should_Match()
    {
      var contactId = 1;

      var spec = new Specification<Contact>(contact => contact.Name == "test" && contact.ContactTypeId == contactId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Two_Part_Predicate_Variable_On_Left_Should_Match()
    {
      var contactId = 1;

      var spec = new Specification<Contact>(contact => contact.Name == "test" && contactId == contact.ContactTypeId);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }

    [Fact]
    public void Partition_Column_Used_More_Than_Once_Should_Not_Match()
    {
      var contactId = 1;

      var spec = new Specification<Contact>(contact => contact.ContactTypeId == 1 || contactId == 2);
      var cachingStrategy = new StandardCachingStrategyWithPartition<Contact, int, int>(cacheProvider, c => c.ContactTypeId);

      var isMatch = cachingStrategy.TryPartitionValue(spec, out int value);

      isMatch.Should().Be(true);
      value.Should().Be(1);
    }
  }
}
