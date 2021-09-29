using System.Threading.Tasks;

namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public interface ICommandHandlerAsync<in TCommand> where TCommand : ICommand
  {
    Task<CommandResponse> HandleAsync(TCommand command);
  }
}
