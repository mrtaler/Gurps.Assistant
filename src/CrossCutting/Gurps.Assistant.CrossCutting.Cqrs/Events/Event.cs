using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Events
{
  public abstract class Event : IEvent
  {
    public string UserId { get; set; }
    public string Source { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
  }
}
