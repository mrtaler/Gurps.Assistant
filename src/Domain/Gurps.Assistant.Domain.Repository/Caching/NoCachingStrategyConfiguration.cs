using Gurps.Assistant.Domain.Repository.Configuration;

namespace Gurps.Assistant.Domain.Repository.Caching
{
  public class NoCachingStrategyConfiguration : CachingStrategyConfiguration
  {
    public NoCachingStrategyConfiguration(string name)
    {
      Name = name;
      Factory = typeof(NoCachingConfigCachingStrategyFactory);
    }
  }
}
