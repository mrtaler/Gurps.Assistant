using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.InMemory.RolePlayedGamesHelper.Repository.InMemoryRepository.SharpRepository;
using Gurps.Assistant.Domain.Repository.Traits;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Traits
{

  public class ICanGetTraitTests : TestBase
  {
    [Fact]
    public void ICanGet_Exposes_Get_By_Id()
    {
      var repo = new ContactRepository(
        new ConcurrentDictionary<int, Contact>());

      var contact = new Contact { Name = "Test User", ContactTypeId = 1 };
      repo.Add(contact);

      var result = repo.Get(contact.ContactId);
      result.Name.Should().Be(contact.Name);
      result.ContactTypeId.Should().Be(contact.ContactTypeId);
    }

    [Fact]
    public void ICanGet_Exposes_GetAll()
    {
      var repo = new ContactRepository(
        new ConcurrentDictionary<int, Contact>());

      for (int i = 1; i <= 5; i++)
      {
        var contact = new Contact { Name = "Test User " + i };
        repo.Add(contact);
      }

      IEnumerable<Contact> result = repo.GetAll().ToList();
      result.Count().Should().Be(5);
    }

    [Fact]
    public void ICanGet_Exposes_Get_With_Result()
    {
      var repo = new ContactRepository(
        new ConcurrentDictionary<int, Contact>());

      var contact = new Contact { Name = "Test User" };
      repo.Add(contact);

      var result = repo.Get(1, x => new { contact.ContactId, contact.Name });
      result.Name.Should().Be(contact.Name);
    }

    private class ContactRepository : InMemRepository<Contact, int>, IContactRepository
    {
      /// <inheritdoc />
      public ContactRepository(
          ConcurrentDictionary<int, Contact> dataContextFactory,
          ICachingStrategy<Contact, int> cachingStrategy = null)
          : base(dataContextFactory, cachingStrategy)
      {
      }
    }

    private interface IContactRepository : ICanGet<Contact, int>
    {
    }
  }
}