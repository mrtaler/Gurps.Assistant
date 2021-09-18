using System.Security.Cryptography.X509Certificates;
using Autofac;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Gurps.Assistant.Domain.Repository.RavenDb;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
{
  public class RavenContextTestModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
          .Register((c) =>
          {
            /* var x509 = new X509Store(StoreName.My, StoreLocation.LocalMachine);
             x509.Open(OpenFlags.ReadOnly);
             var certificateSt = x509.Certificates.Find(X509FindType.FindByThumbprint, "4eda9df28ca6a3e3d15e59e91ae18e345824c3af", false)[0];
             */
            var certificateSt = new X509Certificate2(@"C:\CD\Certs\Raven\TalerRavenAzure.pfx",
                                                           "123456789", X509KeyStorageFlags.MachineKeySet);

            var store = new DocumentStore
            {
              Urls = new[] { "https://a.mrtaler.development.run" },
              Database = "GurpsData",
              Conventions = { IdentityPartsSeparator = '-' },
              Certificate = certificateSt
            };
            return store.Initialize();
            ;
          })
          .As<IDocumentStore>()
          .SingleInstance();

      builder
          .Register(
              x =>
                  new RavenDbContextFactory(x.Resolve<IDocumentStore>()))
          .AsSelf()
          .As<IDataContextFactory<IDocumentSession>>().SingleInstance();

      builder.RegisterType<RavenUnitOfWork>().As<IUnitOfWork<IDocumentSession, RavenDbContextFactory>>().AsSelf();
    }
  }
}