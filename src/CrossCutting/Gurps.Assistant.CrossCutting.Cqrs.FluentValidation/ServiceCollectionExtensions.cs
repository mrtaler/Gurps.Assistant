using Gurps.Assistant.CrossCutting.Cqrs.Extensions;
using Gurps.Assistant.CrossCutting.Cqrs.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Gurps.Assistant.CrossCutting.Cqrs.FluentValidation
{
  public static class ServiceCollectionExtensions
  {
    public static IKledexServiceBuilder AddFluentValidationProvider(this IKledexServiceBuilder builder)
    {
      builder.Services.AddTransient<IValidationProvider, FluentValidationProvider>();

      return builder;
    }
  }
}