using System.Collections.Concurrent;
using System.Linq;
using Gurps.Assistant.Domain.Repository.UnitTests.PrimaryKey;
using Gurps.Assistant.Domain.Repository.UnitTests.TestObjects;
using Xunit;

namespace Gurps.Assistant.Domain.Repository.UnitTests.Aspects
{

  public class RepositoryActionBaseAttributeTests
  {
    [Fact]
    public void Aspect_IsNotCalled_WhenDisabled()
    {
      //Arrange
      var repository = new TestRepository<Product, int>(
         new ConcurrentDictionary<int, Product>());
      var aspect = repository
          .GetAspects()
          .OfType<AuditAttributeMock>()
          .First();

      //Act
      repository.SuppressAudit();
      var product = repository.Get(1);


      //Assert
      Assert.True(aspect.OnInitializedCalled);
      Assert.False(aspect.Enabled);
      Assert.False(aspect.OnGetExecutingCalled);
      Assert.False(aspect.OnGetExecutedCalled);
    }

    [Fact]
    public void Aspect_IsCalled_WhenReEnabled()
    {
      //Arrange
      var repository = new TestRepository<Product, int>(
              new ConcurrentDictionary<int, Product>());
      var aspect = repository
          .GetAspects()
          .OfType<AuditAttributeMock>()
          .First();

      //Act
      repository.SuppressAudit();
      repository.RestoreAudit();
      var product = repository.Get(1);


      //Assert
      Assert.True(aspect.OnInitializedCalled);
      Assert.True(aspect.Enabled);
      Assert.True(aspect.OnGetExecutingCalled);
      Assert.True(aspect.OnGetExecutedCalled);
    }

    [Fact]
    public void Aspect_WhenMultipleApplied_ExecutedInOrder()
    {
      //Arrange
      var repository = new TestRepository<Product, int>(
              new ConcurrentDictionary<int, Product>());
      var aspects = repository
          .GetAspects()
          .ToArray();
      var audit = (AuditAttributeMock)aspects.First(a => a is AuditAttributeMock);
      var specificAudit = (SpecificAuditAttribute)aspects.First(a => a is SpecificAuditAttribute);

      //Act
      var product = repository.Get(1);

      //Assert
      Assert.True(specificAudit.ExecutedOn > audit.ExecutedOn);//.Greater(specificAudit.ExecutedOn, audit.ExecutedOn);
    }
  }
}
