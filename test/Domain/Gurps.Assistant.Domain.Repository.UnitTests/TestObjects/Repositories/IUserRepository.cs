using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects.Repositories
{
  public interface IUserRepository : IRepository<User, string>
  {
  }
}