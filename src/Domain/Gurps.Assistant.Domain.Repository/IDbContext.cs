using System;

namespace Gurps.Assistant.Domain.Repository
{
  public interface IDbContext : IDisposable
  {
    int SaveChanges();
  }
}
