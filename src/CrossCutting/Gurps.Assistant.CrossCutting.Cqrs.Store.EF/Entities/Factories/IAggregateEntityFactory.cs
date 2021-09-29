using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities.Factories
{
  public interface IAggregateEntityFactory
  {
    AggregateEntity CreateAggregate(Type aggregateType, Guid aggregateRootId);
  }
}