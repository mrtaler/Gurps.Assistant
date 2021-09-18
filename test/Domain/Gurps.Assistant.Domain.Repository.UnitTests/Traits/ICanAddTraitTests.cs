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

  public class ICanAddTraitTests : TestBase
  {
    [Fact]
    public void ICanAdd_Exposes_Add_Entity()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());
      repo.Add(new Contact());
    }

    [Fact]
    public void ICanAdd_Exposes_Add_Multiple_Entities()
    {
      var repo = new ContactRepository(
                                           new ConcurrentDictionary<int, Contact>());
      repo.Add(new List<Contact> { new Contact(), new Contact() });
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

    private interface IContactRepository : ICanAdd<Contact>
    {
    }
  }
}