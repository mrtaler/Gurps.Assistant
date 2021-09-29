using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Domain
{
  public class TestAggregateCreated : DomainEvent
  {
    public int Number { get; set; }

    public TestAggregateCreated(int number) => Number = number;
  }
}