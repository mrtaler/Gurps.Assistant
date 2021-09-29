using System;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Gurps.Assistant.CrossCutting.Cqrs.Extensions;
using Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb
{
  public static class ServiceCollectionExtensions
  {
    public static IKledexServiceBuilder AddRavenDbdataProvider(this IKledexServiceBuilder builder)
    {
      return builder.AddRavenDbDataProvider(opt => { });
    }

    public static IKledexServiceBuilder AddRavenDbDataProvider(
        this IKledexServiceBuilder builder,
        Action<DomainDbOptions> setupAction)
    {
      if (builder == null)
      {
        throw new ArgumentNullException(nameof(builder));
      }

      if (setupAction == null)
      {
        throw new ArgumentNullException(nameof(setupAction));
      }

      builder.Services.Configure(setupAction);

      builder.Services
          .AddTransient<IStoreProvider, StoreProvider>();

      builder.Services
          .AddTransient<IAggregateDocumentFactory, AggregateDocumentFactory>()
          .AddTransient<ICommandDocumentFactory, CommandDocumentFactory>()
          .AddTransient<IEventDocumentFactory, EventDocumentFactory>();


      /* builder.Services
              .AddTransient<IRepositoryAsync<AggregateDocument, string>, EfCoreRepository<AggregateDocument, string>>();
       builder.Services
              .AddTransient<IRepositoryAsync<CommandDocument, string>, EfCoreRepository<CommandDocument, string>>();
       builder.Services
              .AddTransient<IRepositoryAsync<EventDocument, string>, EfCoreRepository<EventDocument, string>>();
*/



      return builder;
    }
  }
}
