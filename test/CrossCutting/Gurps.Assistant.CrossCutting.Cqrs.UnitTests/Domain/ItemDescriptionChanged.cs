using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class ItemDescriptionChanged : DomainEvent
  {
    public int ItemId { get; set; }
    public string Description { get; set; }
  }
}