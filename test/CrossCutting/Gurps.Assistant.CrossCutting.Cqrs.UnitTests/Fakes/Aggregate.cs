using Gurps.Assistant.CrossCutting.Cqrs.Domain;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes
{
  public class Aggregate : AggregateRoot
  {
    public Aggregate()
    {
      AddAndApplyEvent(new AggregateCreated());
    }

    private void Apply(AggregateCreated @event)
    {
    }
  }
}
