using System;
using System.Linq;
using Gurps.Assistant.CrossCutting.Cqrs.Configuration;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Gurps.Assistant.CrossCutting.Cqrs.Extensions
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Adds Kledex. 
    /// Pass any of the types that are contained in the assemblies where your messages and handlers are.
    /// One for each assembly.
    /// e.g. typeOf(CreatePost) where CreatePost is one of your commands.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="types">The types.</param>
    public static IKledexServiceBuilder AddKledex(this IServiceCollection services, params Type[] types)
    {
      return services.AddKledex(opt => { }, types);
    }

    /// <summary>
    /// Adds Kledex. 
    /// Pass any of the types that are contained in the assemblies where your messages and handlers are.
    /// One for each assembly.
    /// e.g. typeOf(CreatePost) where CreatePost is one of your commands.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="setupAction">The options.</param>
    /// <param name="types">The types.</param>
    public static IKledexServiceBuilder AddKledex(this IServiceCollection services, Action<Options> setupAction, params Type[] types)
    {
      if (services == null)
        throw new ArgumentNullException(nameof(services));

      var typeList = types.ToList();
      typeList.Add(typeof(IDispatcher));

      services.Scan(s => s
          .FromAssembliesOf(typeList)
          .AddClasses()
          .AsImplementedInterfaces());

      services.AddTransient(typeof(IEventStoreRepository<>), typeof(EventStoreRepository<>));

      services.AddAutoMapper(typeList);

      services.Configure(setupAction);

      return new KledexServiceBuilder(services);
    }

    [Obsolete("Please use an override of AddKledex method.")]
    public static IKledexServiceBuilder AddOptions(this IKledexServiceBuilder builder, Action<Options> setupAction)
    {
      if (builder == null)
        throw new ArgumentNullException(nameof(builder));

      if (setupAction == null)
        throw new ArgumentNullException(nameof(setupAction));

      builder.Services.Configure(setupAction);

      return builder;
    }
  }
}
