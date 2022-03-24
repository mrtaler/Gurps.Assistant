using Gurps.Assistant.Domain.Repository.Configuration;

namespace Gurps.Assistant.Domain.Repository.Caching
{
  public class NoCachingConfigCachingProviderFactory : ConfigCachingProviderFactory
  {
    public NoCachingConfigCachingProviderFactory(ICachingProviderConfiguration config)
        : base(config)
    {
    }

    public override ICachingProvider GetInstance()
    {
      return new NoCachingProvider();
    }
  }
}
