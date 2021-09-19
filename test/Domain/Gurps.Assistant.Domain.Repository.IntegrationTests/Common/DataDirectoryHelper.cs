using System.IO;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Common
{
  public class DataDirectoryHelper
  {
    public static string GetDataDirectory()
    {
      var rd = new CurrentDirectory();
      rd.MoveUpToDirectory("RolePlayedGamesHelper.Repository.IntegrationTests");
      var path = Path.Combine(rd.FullName, @"Data");

      if (!Directory.Exists(path))
      {
        Directory.CreateDirectory(path);
      }

      return path;
    }
  }
}