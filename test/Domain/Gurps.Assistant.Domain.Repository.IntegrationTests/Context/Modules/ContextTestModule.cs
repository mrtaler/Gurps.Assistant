using Autofac;
using Gurps.Assistant.Domain.Repository.EntityFrameworkCore;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
using Gurps.Assistant.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
{
  public class ContextCoreTestModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder
          .Register((c) => new DbContextOptionsBuilder<TestObjectContextCore>()
                           .UseInMemoryDatabase("TestDataBase")
                           .Options)
          .AsSelf()
          .SingleInstance();

      builder
          .RegisterType<DbCoreContextFactory<TestObjectContextCore>>()
          .AsSelf()
          .As<IDataContextFactory<TestObjectContextCore>>().SingleInstance();

      builder.RegisterType<DbCoreUnitOfWork<TestObjectContextCore>>()
             .As<IUnitOfWork<TestObjectContextCore, DbCoreContextFactory<TestObjectContextCore>>>()
             .AsSelf();
    }

    /*   protected override void Load(ContainerBuilder builder)
       {

           /*builder
              .Register(x => new EfCoreContextFactory()).
              As<IDataContextFactory<TestObjectContextCore>>()
              .SingleInstance();

          builder
              .RegisterType<UnitOfWork<TestObjectContextCore>>()
              .As<IUnitOfWork>()
              .InstancePerLifetimeScope();

          builder
              .RegisterType<EfCoreRepository<Contact, TestObjectContextCore>>()
              .As<IRepository<Contact>>()
              .InstancePerDependency();
*/
    //      }
  }
}