using Autofac;
using Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;
using Gurps.Assistant.Domain.Repository.RavenDb;
using System;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context
{
  public class RavenContextTest : TestBase
  {
    private readonly IContainer container;

    public RavenContextTest()
    {
      var buider = new ContainerBuilder();
      buider.RegisterModule<RavenContextTestModule>();
      container = buider.Build();

    }

    [SkippableFact]
    public void RavenDbContextTest()
    {
      Skip.IfNot(Environment.OSVersion.VersionString.Contains("Windows"));
      var uow = container.Resolve<RavenUnitOfWork>();
      var repo = uow.GetRepository<Contact, string>();
      var repo1 = uow.GetRepository<EmailAddress, string>();
      //var repo = container.Resolve<IRepository<Contact>>();
      repo.Add(new Contact
      {
        ContactId = 1,
        Name = "str"

      });
      repo1.Add(new EmailAddress
      {
        ContactId = 1,
        Email = "123123@epam.com",
        EmailAddressId = 1,
        Label = "asdasd"
      });
      uow.SaveChanges();

      var tm1 = repo1.GetAll();
    }
  }
}
