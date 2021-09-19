using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.IntegrationTests.TestObjects.Repositories
{
  public interface IUserRepository : IRepository<User, string>
  {
  }
}