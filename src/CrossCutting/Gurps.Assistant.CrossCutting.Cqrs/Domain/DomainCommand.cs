using System;
using Gurps.Assistant.CrossCutting.Cqrs.Commands;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public abstract class DomainCommand<TAggregateRoot> : Command, IDomainCommand<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
  {
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AggregateRootId { get; set; } = Guid.NewGuid();
    public int? ExpectedVersion { get; set; }
    public bool? SaveCommandData { get; set; }
  }
}
