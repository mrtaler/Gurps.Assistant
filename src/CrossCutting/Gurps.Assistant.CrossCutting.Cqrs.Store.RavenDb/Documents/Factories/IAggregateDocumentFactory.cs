using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.RavenDb.Documents.Factories
{
  public interface IAggregateDocumentFactory
  {
    AggregateDocument CreateAggregate(Type aggregateType, Guid aggregateRootId);
  }
}