using System;
using Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Assert;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Common
{
  public class CurrentDirectory : RelativeDirectory
  {
    /// <summary>
    ///  Sets the relative directory location based on the environment's current directory.
    ///  Often this is the project's debug location when running locally.
    /// </summary>
    public CurrentDirectory() : base(AppDomain.CurrentDomain.BaseDirectory)
    {
    }
  }
}