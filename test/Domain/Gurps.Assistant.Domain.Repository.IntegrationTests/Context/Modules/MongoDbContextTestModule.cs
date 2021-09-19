using Autofac;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Gurps.Assistant.Domain.Repository.MongoDb;
using MongoDB.Driver;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
{
  public class MongoDbContextTestModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
          .Register((c) => new MongoDbConfiguration
          {
            ConnectionString = "mongodb://admin:password@localhost:27017",
            DatabaseName = "TestMongoDB"
          })
          .AsSelf()
          .SingleInstance();

      builder
          .RegisterType<MongoDbContextFactory>()
          .AsSelf()
          .As<IDataContextFactory<IMongoClient>>().SingleInstance();

      builder.RegisterType<MongoDbUnitOfWork>().As<IUnitOfWork<IMongoClient, MongoDbContextFactory>>().AsSelf();
    }
  }
}