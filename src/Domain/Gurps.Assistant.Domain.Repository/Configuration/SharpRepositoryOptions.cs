using Microsoft.Extensions.Options;

namespace Gurps.Assistant.Domain.Repository.Configuration
{
  public class SharpRepositoryOptions : IOptions<SharpRepositoryConfiguration>
  {
    public SharpRepositoryOptions(SharpRepositoryConfiguration configuration)
    {
      Value = configuration;
    }

    public SharpRepositoryConfiguration Value { get; protected set; }
  }
}
