using System;

namespace Gurps.Assistant.Domain.Repository.Configuration
{
  public class ConfigurationErrorsException : Exception
  {
    public ConfigurationErrorsException()
        : base()
    { }

    public ConfigurationErrorsException(string message)
        : base(message)
    { }

    public ConfigurationErrorsException(string message, Exception innerException)
        : base(message, innerException)
    { }
  }
}
