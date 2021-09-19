//using System;
//using System.Data.Common;
//using System.Data.SqlClient;
//using Autofac;
//using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects;
//using Gurps.Assistant.Domain.Repository.Interfaces;

//namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Context.Modules
//{
//  public class ContextEfTestModule : Module
//  {
//    protected override void Load(ContainerBuilder builder)
//    {
//      builder
//        .Register((c) =>
//                  {
//                    var db = new SqlConnection();
//                    Console.WriteLine(Environment.GetEnvironmentVariable("mssql"));
//                    //  db.ConnectionString = $"Data Source={EfDataDirectoryFactory.Build()}";
//                    db.ConnectionString = Environment.GetEnvironmentVariable("mssql"); ;
//                    return db;
//                  })
//        .As<DbConnection>()
//             .AsImplementedInterfaces()
//             .SingleInstance();

//      builder
//          .RegisterType<DbEfContextFactory<TestObjectContext>>()
//          .AsSelf()
//          .As<IDataContextFactory<TestObjectContext>>().SingleInstance();

//      builder.RegisterType<DbEfUnitOfWork<TestObjectContext>>()
//             .As<IUnitOfWork<TestObjectContext, DbEfContextFactory<TestObjectContext>>>()
//             .AsSelf();
//    }

//    /*   protected override void Load(ContainerBuilder builder)
//       {

//           /*builder
//              .Register(x => new EfCoreContextFactory()).
//              As<IDataContextFactory<TestObjectContextCore>>()
//              .SingleInstance();

//          builder
//              .RegisterType<UnitOfWork<TestObjectContextCore>>()
//              .As<IUnitOfWork>()
//              .InstancePerLifetimeScope();

//          builder
//              .RegisterType<EfCoreRepository<Contact, TestObjectContextCore>>()
//              .As<IRepository<Contact>>()
//              .InstancePerDependency();
//*/
//    //      }
//  }
//}