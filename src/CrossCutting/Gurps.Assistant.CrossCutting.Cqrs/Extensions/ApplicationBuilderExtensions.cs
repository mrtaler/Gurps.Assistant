using Microsoft.AspNetCore.Builder;

namespace Gurps.Assistant.CrossCutting.Cqrs.Extensions
{
  public static class ApplicationBuilderExtensions
  {
    public static IKledexAppBuilder UseKledex(this IApplicationBuilder app)
    {
      return new KledexAppBuilder(app);
    }
  }
}