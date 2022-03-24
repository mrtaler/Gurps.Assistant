using Gurps.Assistant.CrossCutting.Cqrs.Bus;
using Gurps.Assistant.CrossCutting.Cqrs.Events;
using System;
using System.Collections.Generic;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes
{
  public class SomethingCreated : Event, IBusQueueMessage
  {
    public DateTime? ScheduledEnqueueTimeUtc { get; set; }
    public string QueueName { get; set; } = "queue-name";
    public IDictionary<string, object> Properties { get; set; }
  }
}
