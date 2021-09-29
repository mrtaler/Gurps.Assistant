using System;
using Gurps.Assistant.CrossCutting.Cqrs.Events;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public interface IDomainEvent : IEvent
  {
    Guid Id { get; set; }
    Guid AggregateRootId { get; set; }
    Guid CommandId { get; set; }
    void Update(IDomainCommand command);
  }
}
