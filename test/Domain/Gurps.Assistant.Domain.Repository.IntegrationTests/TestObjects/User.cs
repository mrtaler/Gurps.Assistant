using Gurps.Assistant.Domain.Repository.Attributes;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects
{
  public class User
  {
    [RepositoryPrimaryKey(Order = 1)]
    public string Username { get; set; }

    [RepositoryPrimaryKey(Order = 1)]
    public int Age { get; set; }

    public string FullName { get; set; }

    public int ContactTypeId { get; set; }
  }

  public class UserBrief
  {
    public string Id { get; set; }
    public string Email { get; set; }
  }

}