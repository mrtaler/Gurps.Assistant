using Microsoft.AspNetCore.Builder;

namespace Gurps.Assistant.CrossCutting.Cqrs.Extensions
{
  public interface IKledexAppBuilder
  {
    IApplicationBuilder App { get; }
  }
}