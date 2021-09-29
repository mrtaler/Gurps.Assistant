using System;
using System.Security.Cryptography.X509Certificates;
using Autofac;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Gurps.Assistant.Domain.Repository.RavenDb;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Session;
using Raven.Client.Exceptions;
using Raven.Client.Exceptions.Database;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
{
  public class RavenContextTestModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
          .Register((c) =>
          {
            var store = new DocumentStore
            {
              Urls = new[] { "http://localhost:8080" },
              Database = "GurpsData",
              Conventions = { IdentityPartsSeparator = '-' }
            };
            var documentStore = store.Initialize();

            EnsureDatabaseExists(documentStore);

            return documentStore;
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
    public void EnsureDatabaseExists(IDocumentStore store, string database = null, bool createDatabaseIfNotExists = true)
    {
      database = database ?? store.Database;

      if (string.IsNullOrWhiteSpace(database))
        throw new ArgumentException("Value cannot be null or whitespace.", nameof(database));

      try
      {
        store.Maintenance.ForDatabase(database).Send(new GetStatisticsOperation());
      }
      catch (DatabaseDoesNotExistException)
      {
        if (createDatabaseIfNotExists == false)
          throw;

        try
        {
          store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(database)));
        }
        catch (ConcurrencyException)
        {
          // The database was already created before calling CreateDatabaseOperation
        }

      }
    }
  }
}