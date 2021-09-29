namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public interface IDomainDbContextFactory
  {
    DomainDbContext CreateDbContext();
  }
}