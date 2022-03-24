using Gurps.Assistant.Domain.Repository.Caching;

namespace Gurps.Assistant.Domain.Repository.Configuration
{
  public interface IConfigCachingProviderFactory
  {
    ICachingProvider GetInstance();
  }

  public abstract class ConfigCachingProviderFactory : IConfigCachingProviderFactory
  {
    protected ICachingProviderConfiguration CachingProviderConfiguration;

    protected ConfigCachingProviderFactory(ICachingProviderConfiguration config)
    {
      CachingProviderConfiguration = config;
    }

    public abstract ICachingProvider GetInstance();
  }
}
