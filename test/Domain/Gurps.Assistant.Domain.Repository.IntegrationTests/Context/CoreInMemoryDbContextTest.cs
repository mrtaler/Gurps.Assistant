using Autofac;
using Gurps.Assistant.Domain.Repository.EntityFrameworkCore;
using Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context
{
  public class CoreInMemoryDbContextTest : TestBase
  {
    private readonly IContainer container;

    public CoreInMemoryDbContextTest()
    {
      var buider = new ContainerBuilder();
      buider.RegisterModule<ContextCoreTestModule>();
      container = buider.Build();

    }

    [Fact]
    public void CoreInMemoryContextTest()
    {
      var uow = container.Resolve<IUnitOfWork<TestObjectContextCore, DbCoreContextFactory<TestObjectContextCore>>>();
      var repo = uow.GetRepository<Contact, string>();
      var repo1 = uow.GetRepository<EmailAddress, string>();
      //var repo = container.Resolve<IRepository<Contact>>();
      repo.Add(new Contact
      {
        ContactId = 2,
        Name = "str"

      });
      repo1.Add(new EmailAddress()
      {
        ContactId = 2,
        Email = "123123@epam.com",
        EmailAddressId = 1,
        Label = "asdasd"
      });
      uow.SaveChanges();
      _ = repo1.GetAll();
    }
  }
}
