//using Autofac;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;
//using Gurps.Assistant.Domain.Repository.Interfaces;
//using Xunit;

//namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context
//{
//  public class EfInMemoryDbContextTest : TestBase
//  {
//    private readonly IContainer container;

//    public EfInMemoryDbContextTest()
//    {
//      new LaunchSettingsFixture();
//      var buider = new ContainerBuilder();
//      buider.RegisterModule<ContextEfTestModule>();
//      container = buider.Build();

//    }

//    [Fact]
//    public void EfInMemoryContextTest()
//    {
//      var uow = container.Resolve<IUnitOfWork<TestObjectContext, DbEfContextFactory<TestObjectContext>>>();
//      var repo = uow.GetRepository<Contact, int>();
//      var repo1 = uow.GetRepository<EmailAddress, int>();
//      //var repo = container.Resolve<IRepository<Contact>>();
//      repo.Add(new Contact
//      {
//        ContactId = 1,
//        Name = "str",
//        ContactType = new ContactType() { Abbreviation = "str", Name = "mt" }

//      });
//      repo1.Add(new EmailAddress()
//      {
//        ContactId = 1,
//        Email = "123123@epam.com",
//        EmailAddressId = 1,
//        Label = "asdasd"
//      });
//      uow.SaveChanges();

//      var tm1 = repo1.GetAll();
//    }
//  }
//}
