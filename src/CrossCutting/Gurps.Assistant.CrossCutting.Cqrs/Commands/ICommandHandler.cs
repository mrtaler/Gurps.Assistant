namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public interface ICommandHandler<in TCommand> where TCommand : ICommand
  {
    CommandResponse Handle(TCommand command);
  }
}