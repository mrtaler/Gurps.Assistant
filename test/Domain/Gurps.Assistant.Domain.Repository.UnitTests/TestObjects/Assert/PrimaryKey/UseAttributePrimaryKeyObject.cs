using Gurps.Assistant.Domain.Repository.Attributes;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert.PrimaryKey
{
  internal class UseAttributePrimaryKeyObject
  {
    public UseAttributePrimaryKeyObject()
    {

    }
    [RepositoryPrimaryKey]
    public int SomeRandomName { get; set; }
    public string Value { get; set; }
  }
}