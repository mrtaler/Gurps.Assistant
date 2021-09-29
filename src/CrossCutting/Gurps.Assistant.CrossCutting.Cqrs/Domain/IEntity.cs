using System;

namespace Gurps.Assistant.CrossCutting.Cqrs.Domain
{
  public interface IEntity
  {
    Guid Id { get; }
  }
}
