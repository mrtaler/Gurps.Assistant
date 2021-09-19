//using Autofac;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Mongo;
//using Gurps.Assistant.Domain.Repository.MongoDb;
//using System;
//using Xunit;

//namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context
//{
//  public class MongoDbContextTest : TestBase
//  {
//    private readonly IContainer container;

//    public MongoDbContextTest()
//    {
//      var buider = new ContainerBuilder();
//      buider.RegisterModule<MongoDbContextTestModule>();
//      container = buider.Build();

//    }

//    [SkippableFact]
//    public void MongoContextTest()
//    {
//      Skip.IfNot(Environment.OSVersion.VersionString.Contains("Windows"));
//      var uow = container.Resolve<MongoDbUnitOfWork>();
//      var repo = uow.GetRepository<ContactMongo, string>();
//      var repo1 = uow.GetRepository<EmailAddressMongo, string>();
//      //var repo = container.Resolve<IRepository<Contact>>();
//      repo.Add(new ContactMongo
//      {
//        ContactId = 2,
//        Name = "str"

//      });
//      repo1.Add(new EmailAddressMongo()
//      {
//        ContactId = 2,
//        Email = "123123@epam.com",
//        EmailAddressId = 1,
//        Label = "asdasd"
//      });
//      uow.SaveChanges();

//      var tm1 = repo1.GetAll();
//    }
//  }
//}
