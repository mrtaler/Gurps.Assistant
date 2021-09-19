using Gurps.Assistant.Domain.Repository.Configuration;

namespace Gurps.Assistant.Domain.Repository.Caching
{
  public class StandardCachingStrategyConfiguration : CachingStrategyConfiguration
  {
    public StandardCachingStrategyConfiguration(string name) : this(name, true, true, null)
    {
    }

    public StandardCachingStrategyConfiguration(string name, bool writeThroughCachingEnabled, bool generationalCachingEnabled)
        : this(name, writeThroughCachingEnabled, generationalCachingEnabled, null)
    {
    }

    public StandardCachingStrategyConfiguration(string name, bool writeThroughCachingEnabled, bool generationalCachingEnabled, int? maxResults = null)
    {
      Name = name;
      WriteThroughCachingEnabled = writeThroughCachingEnabled;
      GeneraltionalCachingEnabled = generationalCachingEnabled;
      MaxResults = maxResults;
      Factory = typeof(StandardConfigCachingStrategyFactory);

    }

    public bool WriteThroughCachingEnabled
    {
      set { Attributes["writeThrough"] = value.ToString(); }
    }

    public bool GeneraltionalCachingEnabled
    {
      set { Attributes["generational"] = value.ToString(); }
    }
  }
}
