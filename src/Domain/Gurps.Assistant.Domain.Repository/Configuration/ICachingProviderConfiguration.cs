using System;
using System.Collections.Generic;
using Gurps.Assistant.Domain.Repository.Caching;

namespace Gurps.Assistant.Domain.Repository.Configuration
{
  public interface ICachingProviderConfiguration
  {
    string Name { get; set; }
    Type Factory { get; set; }
    IDictionary<string, string> Attributes { get; set; }
    string this[string key] { get; }

    ICachingProvider GetInstance();
  }
}
