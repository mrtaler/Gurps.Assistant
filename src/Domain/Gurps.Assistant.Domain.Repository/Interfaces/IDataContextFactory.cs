namespace Gurps.Assistant.Domain.Repository.Interfaces
{
  public interface IDataContextFactory<out TContext>
      where TContext : class
  {
    TContext GetContext();
  }

}