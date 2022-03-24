using System;
using System.Collections.Generic;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public class SaveStoreData
  {
    public Type AggregateType { get; set; }
    public Guid AggregateRootId { get; set; }
    public IDomainCommand DomainCommand { get; set; }
    public IEnumerable<IDomainEvent> Events { get; set; }
  }
}
