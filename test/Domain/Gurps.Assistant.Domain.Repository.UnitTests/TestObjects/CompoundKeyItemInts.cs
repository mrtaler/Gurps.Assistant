using Gurps.Assistant.Domain.Repository.Attributes;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  public class CompoundKeyItemInts
  {
    [RepositoryPrimaryKey(Order = 1)]
    public int SomeId { get; set; }

    [RepositoryPrimaryKey(Order = 2)]
    public int AnotherId { get; set; }

    public string Title { get; set; }
  }
}