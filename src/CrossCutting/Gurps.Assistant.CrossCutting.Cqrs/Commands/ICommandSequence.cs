using System.Collections.ObjectModel;

namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public interface ICommandSequence
  {
    ReadOnlyCollection<ICommand> Commands { get; }
  }
}
