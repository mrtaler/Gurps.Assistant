using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Events
{
  public interface IEvent
  {
    string UserId { get; set; }
    string Source { get; set; }
    DateTime TimeStamp { get; set; }
  }
}
