using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities.Factories
{
  public class AggregateEntityFactory : IAggregateEntityFactory
  {
    public AggregateEntity CreateAggregate(Type aggregateType, Guid aggregateRootId) => new AggregateEntity
    {
      Id = aggregateRootId.ToString(),
      Type = aggregateType.AssemblyQualifiedName
    };
  }
}