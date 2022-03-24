using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Gurps.Assistant.CrossCutting.Cqrs.Domain;
using System;
using System.Collections.Generic;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes
{
  public class CreateAggregateBusMessage : DomainCommand<Aggregate>, IBusQueueMessage
  {
    public DateTime? ScheduledEnqueueTimeUtc { get; set; }
    public string QueueName { get; set; } = "create-something";
    public IDictionary<string, object> Properties { get; set; }
  }
}
