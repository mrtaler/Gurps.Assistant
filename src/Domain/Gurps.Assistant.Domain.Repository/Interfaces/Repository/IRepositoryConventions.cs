using System;

namespace Gurps.Assistant.Domain.Repository.Interfaces.Repository
{
  public interface IRepositoryConventions
  {
    Func<Type, string> GetPrimaryKeyName { get; set; }
  }
}
