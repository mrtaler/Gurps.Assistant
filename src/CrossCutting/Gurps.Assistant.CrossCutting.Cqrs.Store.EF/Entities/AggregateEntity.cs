using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities
{
  public class AggregateEntity
  {
    public string Id { get; set; }
    public string Type { get; set; }
  }
}
