using Gurps.Assistant.CrossCutting.Cqrs.Commands;

namespace Gurps.Assistant.CrossCutting.Cqrs.UnitTests.Fakes
{
  public class SampleCommandSequence : CommandSequence
  {
    public SampleCommandSequence()
    {
      AddCommand(new CommandInSequence());
    }
  }
}
