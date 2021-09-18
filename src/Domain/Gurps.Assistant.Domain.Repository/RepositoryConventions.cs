using System;
using Gurps.Assistant.Domain.Repository.Interfaces.Repository;

namespace Gurps.Assistant.Domain.Repository
{
  public class RepositoryConventions : IRepositoryConventions
  {
    public Func<Type, string> GetPrimaryKeyName { get; set; }

    public RepositoryConventions()
    {
      GetPrimaryKeyName = DefaultRepositoryConventions.GetPrimaryKeyName;
    }
  }
}
