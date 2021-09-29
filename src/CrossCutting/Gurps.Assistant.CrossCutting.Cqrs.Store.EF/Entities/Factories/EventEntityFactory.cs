﻿using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using Newtonsoft.Json;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities.Factories
{
  public class EventEntityFactory : IEventEntityFactory
  {
    public EventEntity CreateEvent(IDomainEvent @event, int version)
    {
      return new EventEntity
      {
        Id = @event.Id,
        AggregateId = @event.AggregateRootId,
        CommandId = @event.CommandId,
        Sequence = version,
        Type = @event.GetType().AssemblyQualifiedName,
        Data = JsonConvert.SerializeObject(@event),
        TimeStamp = @event.TimeStamp,
        UserId = @event.UserId,
        Source = @event.Source
      };
    }
  }
}