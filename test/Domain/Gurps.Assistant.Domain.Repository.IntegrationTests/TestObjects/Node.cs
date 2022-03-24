using Gurps.Assistant.Domain.Repository.Attributes;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  public class Node
  {
    [RepositoryPrimaryKey(Order = 1)]
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; }
    public virtual Node Parent { get; private set; }
  }
}