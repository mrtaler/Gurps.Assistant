using Microsoft.Extensions.DependencyInjection;

namespace Gurps.Assistant.CrossCutting.Cqrs.Extensions
{
  public interface IKledexServiceBuilder
  {
    IServiceCollection Services { get; }
  }
}
