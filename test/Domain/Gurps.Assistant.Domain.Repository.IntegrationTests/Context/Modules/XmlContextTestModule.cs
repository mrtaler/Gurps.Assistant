//using Autofac;

//namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
//{
//  public class XmlContextTestModule : Module
//  {
//    private string xmlDirectory;

//    public XmlContextTestModule(string xmlDirectory)
//    {
//      this.xmlDirectory = xmlDirectory;
//    }

//    protected override void Load(ContainerBuilder builder)
//    {
//      builder
//          .Register((c) => new XmlRepositoryConfiguration(
//                        "Test",
//                        xmlDirectory))
//          .AsSelf()
//          .SingleInstance();

//      builder
//          .Register(
//              x =>
//                  new XmlContextFactory(x.Resolve<XmlRepositoryConfiguration>()))
//          .AsSelf()
//          .As<IDataContextFactory<string>>().SingleInstance();

//      builder.RegisterType<XmlUnitOfWork>().As<IUnitOfWork<string, XmlContextFactory>>().AsSelf();
//    }
//  }
//}