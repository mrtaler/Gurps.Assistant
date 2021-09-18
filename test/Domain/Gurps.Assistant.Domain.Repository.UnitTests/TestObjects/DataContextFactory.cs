using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.Domain.Repository.UnitTests.TestObjects
{
  public class DataContextFactory : DbCoreContextFactory<TestObjectContextCore>
  {
    public DataContextFactory(DbContextOptions<TestObjectContextCore> options) : base(options)
    {
    }
  }
}