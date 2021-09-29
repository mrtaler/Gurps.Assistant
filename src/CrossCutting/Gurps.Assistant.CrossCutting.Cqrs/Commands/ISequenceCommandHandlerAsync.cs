﻿using System.Threading.Tasks;

namespace Gurps.Assistant.CrossCutting.Cqrs.Commands
{
  public interface ISequenceCommandHandlerAsync<in TCommand> where TCommand : ICommand
  {
    Task<CommandResponse> HandleAsync(TCommand command, CommandResponse previousStepResponse);
  }
}
