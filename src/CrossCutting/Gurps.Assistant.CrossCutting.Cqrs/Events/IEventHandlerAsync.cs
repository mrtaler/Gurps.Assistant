using System.Threading.Tasks;

namespace Gurps.Assistant.CrossCutting.Cqrs.Events
{
  public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
  {
    Task HandleAsync(TEvent @event);
  }
}
