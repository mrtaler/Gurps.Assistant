using Gurps.Assistant.CrossCutting.Cqrs.Store.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gurps.Assistant.CrossCutting.Cqrs.Store.EF
{
  public class DomainDbContext : DbContext
  {
    public DomainDbContext(DbContextOptions<DomainDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<AggregateEntity>()
             .ToTable("DomainAggregate");

      modelBuilder.Entity<CommandEntity>()
             .ToTable("DomainCommand");

      modelBuilder.Entity<EventEntity>()
             .ToTable("DomainEvent");
    }

    public DbSet<AggregateEntity> Aggregates { get; set; }
    public DbSet<CommandEntity> Commands { get; set; }
    public DbSet<EventEntity> Events { get; set; }
  }
}
