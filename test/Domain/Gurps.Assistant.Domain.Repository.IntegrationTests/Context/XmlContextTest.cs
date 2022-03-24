//using System.Collections.Generic;
//using System.IO;
//using System.Xml.Serialization;
//using Autofac;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;
//using Xunit;

//namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context
//{
//  public class XmlContextTest : TestBase
//  {
//    private IContainer container;

//    private void CreateXmlRepositoriesForTest(string path)
//    {
//      var writer = new StreamWriter(Path.Combine(path, "Contact.xml"), false);
//      var serializer = new XmlSerializer(typeof(List<Contact>));
//      serializer.Serialize(writer, new List<Contact>
//            {
//                new Contact() { ContactId = 1, Name = "Test User" },
//                new Contact() { ContactId = 2, Name = "Test User 2", ContactTypeId = 1},
//                new Contact() { ContactId = 3, Name = "Test User EDITED" }
//            });

//      writer.Close();
//    }
//    ~XmlContextTest()
//    {

//    }
//    public XmlContextTest()
//    {
//      string path = Directory.GetCurrentDirectory();
//      CreateXmlRepositoriesForTest(path);

//      var buider = new ContainerBuilder();
//      buider.RegisterModule(new XmlContextTestModule(path));
//      container = buider.Build();

//    }

//    [Fact]
//    public void XmlDbContextTest()
//    {
//      var uow = container.Resolve<XmlUnitOfWork>();
//      var repo = uow.GetRepository<Contact, string>();
//      //  var repo1 = uow.GetRepository<EmailAddress, string>();
//      //var repo = container.Resolve<IRepository<Contact>>();
//      repo.Add(new Contact
//      {
//        ContactId = 1,
//        Name = "str"

//      });
//      /*    repo1.Add(new EmailAddress
//          {
//              ContactId = 1,
//              Email = "123123@epam.com",
//              EmailAddressId = 1,
//              Label = "asdasd"
//          });*/
//      uow.SaveChanges();

//      //var tm1 = repo1.GetAll();




//    }
//  }
//}