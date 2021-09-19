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
  public class ICanUpdateTraitTests : TestBase
  {
    [Fact]
    public void ICanUpdate_Exposes_Update_Entity()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());

      var contact = new Contact { Name = "Name" };
      repo.Add(contact);

      contact.Name = "New Name";
      repo.Update(contact);
    }

    [Fact]
    public void ICanUpdate_Exposes_Update_Multiple_Entities()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());

      IList<Contact> contacts = new List<Contact>
                                        {
                                            new Contact {Name = "Contact 1"},
                                            new Contact {Name = "Contact 2"},
                                            new Contact {Name = "Contact 3"},
                                        };

      repo.Add(contacts);

      foreach (var contact in contacts)
      {
        contact.Name += " New Name";
      }

      repo.Update(contacts);
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

    private interface IContactRepository : ICanUpdate<Contact>
    {
    }
  }
}