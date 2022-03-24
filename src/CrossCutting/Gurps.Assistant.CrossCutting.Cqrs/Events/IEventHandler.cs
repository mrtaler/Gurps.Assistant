namespace Gurps.Assistant.CrossCutting.Cqrs.Events
{
  public interface IEventHandler<in TEvent> where TEvent : IEvent
  {
    void Handle(TEvent @event);
  }
}
