using System.Collections.Generic;
using Gurps.Assistant.CrossCutting.Cqrs.Events;

namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public class CommandResponse
  {
    public IEnumerable<IEvent> Events { get; set; } = new List<IEvent>();
    public object Result { get; set; }
  }
}
