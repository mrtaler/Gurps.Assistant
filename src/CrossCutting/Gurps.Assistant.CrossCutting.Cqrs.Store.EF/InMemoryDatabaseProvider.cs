using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class InMemoryDatabaseProvider : IDatabaseProvider
  {
    public DomainDbContext CreateDbContext(string connectionString)
    {
      var optionsBuilder = new DbContextOptionsBuilder<DomainDbContext>();
      optionsBuilder.UseInMemoryDatabase(connectionString);

      return new DomainDbContext(optionsBuilder.Options);
    }
  }
}
