using System.Threading.Tasks;

namespace Gurps.Assistant.CrossCutting.Cqrs.Queries
{
  public interface IQueryHandlerAsync<in TQuery, TResult>
      where TQuery : IQuery<TResult>
  {
    Task<TResult> HandleAsync(TQuery query);
  }
}
