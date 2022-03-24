using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class ItemQuantityChanged : DomainEvent
  {
    public int ItemId { get; set; }
    public int Quantity { get; set; }
  }
}