using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities.Factories
{
  public interface IEventEntityFactory
  {
    EventEntity CreateEvent(IDomainEvent @event, int version);
  }
}