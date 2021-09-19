﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.Common
{
  public class LaunchSettingsFixture : IDisposable
  {
    public LaunchSettingsFixture()
    {
      if (!File.Exists("Properties\\launchSettings.json")) return;

      using var file = File.OpenText("Properties\\launchSettings.json");
      var reader = new JsonTextReader(file);
      var jObject = JObject.Load(reader);


      var variables = jObject
                      .GetValue("profiles")
                      //select a proper profile here
                      .SelectMany(profiles => profiles.Children())
                      .SelectMany(profile => profile.Children<JProperty>())
                      .Where(prop => prop.Name == "environmentVariables")
                      .SelectMany(prop => prop.Value.Children<JProperty>())
                      .ToList();

      foreach (var variable in variables)
      {
        Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
      }
    }

    public void Dispose()
    {
      // ... clean up
    }
  }
}
