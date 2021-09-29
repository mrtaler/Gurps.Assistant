﻿namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public interface ISequenceCommandHandler<in TCommand> where TCommand : ICommand
  {
    CommandResponse Handle(TCommand command, CommandResponse previousStepResponse);
  }
}
