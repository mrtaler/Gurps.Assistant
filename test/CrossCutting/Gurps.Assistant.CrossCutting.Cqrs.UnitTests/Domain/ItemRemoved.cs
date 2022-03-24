using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class ItemRemoved : DomainEvent
  {
    public int ItemId { get; set; }
  }
}