namespace Gurps.Assistant.Domain.Repository.Caching
{
  public interface ICachePrefixManager
  {
    int Counter { get; }
    void IncrementCounter();
  }
}
