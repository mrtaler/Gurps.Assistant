using System;
using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Gurps.Assistant.CrossCutting.Cqrs.Extensions;
using Gurps.Assistant.CrossCutting.Cqrs.ServiceBus.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Gurps.Assistant.CrossCutting.Cqrs.ServiceBus
{
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Adds the service bus provider.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IKledexServiceBuilder AddServiceBusProvider(this IKledexServiceBuilder builder)
    {
      if (builder == null)
      {
        throw new ArgumentNullException(nameof(builder));
      }

      builder.Services
             .AddTransient<IBusMessageDispatcher, BusMessageDispatcher>()
             .AddTransient<IBusProvider, BusProvider>()
             .AddTransient<IMessageFactory, MessageFactory>();

      return builder;
    }
  }
}
