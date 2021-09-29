using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class ItemAdded : DomainEvent
  {
    public string Description { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public bool Taxable { get; set; }
  }
}