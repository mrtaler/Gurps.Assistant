using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents.Factories
{
  public interface IEventDocumentFactory
  {
    EventDocument CreateEvent(IDomainEvent @event, long version);
  }
}