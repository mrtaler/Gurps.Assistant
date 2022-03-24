using FluentAssertions;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Assert.PrimaryKey;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.PrimaryKey
{
  public class RepositoryPrimaryKeyAttributeTests : TestBase
  {
    [Fact]
    public void No_Primary_Key_Should_Return_Null()
    {
      var repos = new TestRepository<NoPrimaryKeyObject, int>(null);
      repos.TestGetPrimaryKeyPropertyInfo().Should().BeNull();
    }

    [Fact]
    public void Id_Primary_Key_Should_Return_Id_Property()
    {
      var repos = new TestRepository<IdPrimaryKeyObject, int>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.PropertyType.Should().Be(typeof(int));
      propInfo.Name.Should().Be("Id");
    }

    [Fact]
    public void Id_Primary_Key_With_Wrong_Type_Should_Return_Null()
    {
      var repos = new TestRepository<IdPrimaryKeyObject, string>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.Should().BeNull();
    }

    [Fact]
    public void ClassId_Primary_Key_Should_Return_Id_Property()
    {
      var repos = new TestRepository<ClassNameIdPrimaryKeyObject, int>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.PropertyType.Should().Be(typeof(int));
      propInfo.Name.Should().Be("ClassNameIdPrimaryKeyObjectId");
    }

    [Fact]
    public void ClassId_Primary_Key_With_Wrong_Type_Should_Return_Null()
    {
      var repos = new TestRepository<ClassNameIdPrimaryKeyObject, string>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.Should().BeNull();
    }

    [Fact]
    public void Attribute_Primary_Key_Should_Return_Id_Property()
    {
      var repos = new TestRepository<UseAttributePrimaryKeyObject, int>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.PropertyType.Should().Be(typeof(int));
      propInfo.Name.Should().Be("SomeRandomName");
    }

    [Fact]
    public void Attribute_Primary_Key_With_Wrong_Type_Should_Return_Null()
    {
      var repos = new TestRepository<UseAttributePrimaryKeyObject, string>(null);
      var propInfo = repos.TestGetPrimaryKeyPropertyInfo();

      propInfo.Should().BeNull();
    }
  }
}
