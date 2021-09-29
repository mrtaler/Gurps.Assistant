namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public interface IDatabaseProvider
  {
    DomainDbContext CreateDbContext(string connectionString);
  }
}