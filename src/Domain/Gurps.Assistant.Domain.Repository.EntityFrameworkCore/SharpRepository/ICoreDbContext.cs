using Gurps.Assistant.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Gurps.Assistant.Domain.Repository.EntityFrameworkCore.SharpRepository
{
  public interface ICoreDbContext : IDbContext
  {
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
  }
}
