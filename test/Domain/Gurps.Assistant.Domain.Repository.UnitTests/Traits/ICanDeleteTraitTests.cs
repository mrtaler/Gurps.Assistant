using System.Collections.Concurrent;
using System.Collections.Generic;
using Gurps.Assistant.Domain.Repository.Caching;
using Gurps.Assistant.Domain.Repository.InMemory.RolePlayedGamesHelper.Repository.InMemoryRepository.SharpRepository;
using Gurps.Assistant.Domain.Repository.Traits;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Traits
{

  public class ICanDeleteTraitTests : TestBase
  {
    [Fact]
    public void ICanDelete_Exposes_Delete_Entity()
    {
      var repo = new ContactRepository(new ConcurrentDictionary<int, Contact>());
      var contact = new Contact { Name = "Test User" };
      repo.Add(contact);
      repo.Delete(contact);
    }

    [Fact]
    public void ICanDelete_Exposes_Delete_Multiple_Entities()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());
      var contact = new Contact { Name = "Test User" };
      repo.Add(contact);
      repo.Add(contact);
      repo.Delete(new List<Contact> { contact, contact });
    }

    [Fact]
    public void ICanDelete_Exposes_Delete_By_Id()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());
      var contact = new Contact { Name = "Test User" };
      repo.Add(contact);
      repo.Delete(1);
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

    private interface IContactRepository : ICanDelete<Contact, int>
    {
    }
  }
}